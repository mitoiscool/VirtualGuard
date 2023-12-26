using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

public class CallTranslator : ITranslator
{
    public void Translate(AstExpression instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        //if(method.Resolve())


        switch (((IMethodDescriptor)instr.Operand).Name.ToString().ToLower())
        {
            case "op_equality": // use our cmp instruction, we should see about hashing
                // edit: we don't even need to use our cmp instr, let them hook
                // op_equality and have them go "oh shit" when they realized virtualguard >
                
                // edit: nvm cuz i make them longs to cmp
                
                block.WithContent(Util.BuildHashInstructions(instr, meth, ctx.Runtime)); // use util
                
                block.WithContent(new VmInstruction(VmCode.Cmp),
                    new VmInstruction(VmCode.Ldc_I4, (int)ctx.Runtime.Descriptor.ComparisonFlags.EqFlag),
                    new VmInstruction(VmCode.Xor));
                break;
            
            default:
                block.WithContent(new VmInstruction(VmCode.Call, instr.Operand));
                break;
        }
        
    }

    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Newobj,
            CilCode.Call,
            CilCode.Callvirt
        }.Contains(instr.OpCode.Code);
        
    }
}