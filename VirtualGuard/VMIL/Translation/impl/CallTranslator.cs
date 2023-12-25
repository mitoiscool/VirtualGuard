using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class CallTranslator : ITranslator
{
    public void Translate(AstExpression instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        //if(method.Resolve())

        switch (((IMethodDescriptor)instr.Operand).Name.ToString().ToLower())
        {
            case "op_equality": // use our cmp instruction, we should see about hashing
                block.WithContent(new VmInstruction(VmCode.Cmp));
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, (int)meth.Runtime.Descriptor.ComparisonFlags.EqFlag));
                block.WithContent(new VmInstruction(VmCode.Xor));
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