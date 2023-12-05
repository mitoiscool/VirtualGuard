using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class UnconditionalTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth)
    {
        block.WithContent(
            new VmInstruction(
                VmCode.__jmploc,
                instr.Operand));
    }

    public bool Supports(CilInstruction instr)
    {
        return instr.OpCode.Code == CilCode.Br;
    }
}