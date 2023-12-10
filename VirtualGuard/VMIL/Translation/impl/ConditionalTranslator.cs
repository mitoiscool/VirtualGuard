using System.Reflection.Emit;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ConditionalTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth)
    {
        if (instr.OpCode.Code == CilCode.Brtrue) // inverse flag
            block.WithContent(new VmInstruction(VmCode.Not));

        block.WithContent(new VmInstruction(VmCode.Jz));
    }

    public bool Supports(CilInstruction instr)
    {
        return new[]
        {
            CilCode.Brtrue,
            CilCode.Brfalse
        }.Contains(instr.OpCode.Code);
    }
}