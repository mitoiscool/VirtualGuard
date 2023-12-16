using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class MemberOpTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Ldtoken:
                block.WithContent(new VmInstruction(VmCode.Ldtoken, instr.Operand));
                break;
        }
    }

    public bool Supports(CilInstruction instr)
    {
        return new[]
        {
            CilCode.Ldtoken
        }.Contains(instr.OpCode.Code);
        
    }
}