using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class BitwiseTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.And:
                block.WithContent(new VmInstruction(VmCode.And));
                break;
            
            case CilCode.Xor:
                block.WithContent(new VmInstruction(VmCode.Xor));
                break;
            
            case CilCode.Or:
                block.WithContent(new VmInstruction(VmCode.Or));
                break;
            
            case CilCode.Not:
                block.WithContent(new VmInstruction(VmCode.Not));
                break;
            
        }
    }

    public bool Supports(CilInstruction instr)
    {
        return new[]
        {
            CilCode.Xor,
            CilCode.Or,
            CilCode.And,
            CilCode.Not
        }.Contains(instr.OpCode.Code);
    }
}