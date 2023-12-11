using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class FieldTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth)
    {
        throw new NotImplementedException();
    }

    public bool Supports(CilInstruction instr)
    {
        throw new NotImplementedException();
    }
}