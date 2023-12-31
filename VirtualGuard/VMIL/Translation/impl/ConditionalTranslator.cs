using System.Reflection;
using System.Reflection.Emit;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.Runtime.OpCodes.impl;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class ConditionalTranslator : ITranslator
{
    [Obfuscation(Feature = "virtualization")]
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        
        
        //block.WithContent(new VmInstruction(VmCode.Ldc_I4, node.ConditionalEdges.First().Target),
        //    new VmInstruction(VmCode.Jz); // aka brtrue, we make brfalse the fallthrough automatically

        //return;
        // tmp fucked impl for testing w hardcoded fallback

        var loc = meth.GetVariableFromArg(3);

        if (instr.OpCode == CilOpCodes.Brtrue)
        {
            // dup flag

            block.WithContent(
                new VmInstruction(VmCode.Dup),
                new VmInstruction(VmCode.Stloc, loc));

            block.WithContent(BuildConditional(node.ConditionalEdges.First().Target, node.UnconditionalEdge.Target, ctx, meth));
            // correct jmp loc onstack

            block.WithContent(new VmInstruction(VmCode.Ldloc, loc));
            
            // get correct entry key
            block.WithContent(BuildConditional(new DynamicStartKeyReference(node.ConditionalEdges.First().Target, true),
                new DynamicStartKeyReference(node.UnconditionalEdge.Target, false), ctx, meth));

            block.WithContent(new VmInstruction(VmCode.Jmp));
        }
        else
        {
            // dup flag

            block.WithContent(
                new VmInstruction(VmCode.Dup),
                new VmInstruction(VmCode.Stloc, loc));

            block.WithContent(BuildConditional(node.UnconditionalEdge.Target, node.ConditionalEdges.First().Target, ctx, meth));
            // correct jmp loc onstack
            
            block.WithContent(new VmInstruction(VmCode.Ldloc, loc));
            
            // get correct entry key
            block.WithContent(BuildConditional(new DynamicStartKeyReference(node.UnconditionalEdge.Target, true),
                new DynamicStartKeyReference(node.ConditionalEdges.First().Target, false), ctx, meth));

            block.WithContent(new VmInstruction(VmCode.Jmp));
        }

        return;
        
        ; // aka brtrue, we make brfalse the fallthrough automatically

        ControlFlowNode<CilInstruction> target = null;
        
        if (instr.OpCode == CilOpCodes.Brtrue)
        {
            // go to condition
            target = node.ConditionalEdges.First().Target;
        }
        else
        { // else invert and let fallback bring us
            target = node.UnconditionalEdge.Target;
        }

        

    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Brtrue,
            CilCode.Brfalse
        }.Contains(instr.OpCode.Code);
    }


    // conditionals will look like
    // jz <loc1>
    // jmp <loc2>
                
    // these can be replaced using arithmetic
                
    // set condition (onstack) into a local
                
    // call sign
    // stloc.0

    // ldc.i4 1 - load 1
    // ldloc.0 - load condition
    // sub
                
    // ldc.i4 <loc1>
    // mul
                
    // stloc.1
                
    // ldc.i4 <loc2>
    // ldloc.0 - load condition
    // mul
                
    // ldloc.1 - load previous
    // add
                
    // jmp - will pop off of stack the correct jmp offset based off of cond

    // 
    
    private VmInstruction[] BuildConditional(object conditional,
        object fallback, VirtualGuardContext ctx, VmMethod method)
    {
        var mathSign = ctx.Module.CorLibTypeFactory.CorLibScope
            .CreateTypeReference("System", "Math")
            .CreateMemberReference("Sign", MethodSignature.CreateStatic(
                ctx.Module.CorLibTypeFactory.Int32, ctx.Module.CorLibTypeFactory.Int32))
            .ImportWith(ctx.Module.DefaultImporter);

        return new[]
        {
            new VmInstruction(VmCode.Call, mathSign), // shouldn't ever be negative
            new VmInstruction(VmCode.Stloc, method.GetVariableFromLocal(0)),

            new VmInstruction(VmCode.Ldc_I4, 1),
            new VmInstruction(VmCode.Ldloc, method.GetVariableFromLocal(0)),
            new VmInstruction(VmCode.Sub),
            
            new VmInstruction(VmCode.Ldc_I4, conditional),
            new VmInstruction(VmCode.Mul),
            
            new VmInstruction(VmCode.Stloc, method.GetVariableFromLocal(1)),
            
            new VmInstruction(VmCode.Ldc_I4, fallback),
            new VmInstruction(VmCode.Ldloc, method.GetVariableFromLocal(0)),
            new VmInstruction(VmCode.Mul),
            
            new VmInstruction(VmCode.Ldloc, method.GetVariableFromLocal(1)),
            new VmInstruction(VmCode.Add),
        };
    }
    
    
}