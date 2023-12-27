using System.Reflection.Emit;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.Runtime.OpCodes.impl;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ConditionalTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        
        // tmp fucked impl for testing w hardcoded fallback

        if (instr.OpCode == CilOpCodes.Brtrue)
        {
            block.WithContent(new VmInstruction(VmCode.Ldc_I4, node.ConditionalEdges.First().Target),
                new VmInstruction(VmCode.Jz),
                new VmInstruction(VmCode.Ldc_I4, node.UnconditionalEdge.Target),
                new VmInstruction(VmCode.Jmp));
        }
        else
        {
            block.WithContent(new VmInstruction(VmCode.Ldc_I4, node.UnconditionalEdge.Target),
                new VmInstruction(VmCode.Jz),
                new VmInstruction(VmCode.Ldc_I4, node.ConditionalEdges.First().Target),
                new VmInstruction(VmCode.Jmp));
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

        block.WithContent(new VmInstruction(VmCode.Ldc_I4, target),
            new VmInstruction(VmCode.Jz)); // aka brtrue, we make brfalse the fallthrough automatically

    }

    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Brtrue,
            CilCode.Brfalse
        }.Contains(instr.OpCode.Code);
    }
}