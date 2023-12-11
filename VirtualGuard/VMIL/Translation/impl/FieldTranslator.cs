using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class FieldTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Stfld:
            case CilCode.Stsfld:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, instr.Operand));
                block.WithContent(new VmInstruction(VmCode.Stfld));
                break;
            
            case CilCode.Ldfld:
            case CilCode.Ldsfld:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, instr.Operand));
                block.WithContent(new VmInstruction(VmCode.Ldfld));
                break;
            
            case CilCode.Ldflda:
            case CilCode.Ldsflda:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, instr.Operand));
                block.WithContent(new VmInstruction(VmCode.Ldflda));
                break;
        }
    }

    public bool Supports(CilInstruction instr)
    {
        return new[]
        {
            CilCode.Ldfld,
            CilCode.Ldsfld,
            CilCode.Ldflda,
            CilCode.Ldsflda,

            CilCode.Stfld,
            CilCode.Stsfld
        }.Contains(instr.OpCode.Code);
    }
}