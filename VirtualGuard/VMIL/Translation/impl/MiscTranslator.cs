using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class MiscTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        block.WithContent(new VmInstruction(VmCode.Dup));
    }

    public bool Supports(CilInstruction instr)
    {
        return instr.OpCode.Code == CilCode.Dup; // lol
    }
}