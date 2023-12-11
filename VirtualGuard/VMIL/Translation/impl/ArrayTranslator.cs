using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ArrayTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth)
    {
        switch (instr.OpCode.Code)
        { 
            case CilCode.Stelem:
            case CilCode.Stelem_I:
            case CilCode.Stelem_I1:
            case CilCode.Stelem_I2:
            case CilCode.Stelem_I4:
            case CilCode.Stelem_I8:
            case CilCode.Stelem_R4:
            case CilCode.Stelem_R8:
            case CilCode.Stelem_Ref:
                block.WithContent(new VmInstruction(VmCode.Stelem));
                break;
            
            case CilCode.Ldelem:
            case CilCode.Ldelem_I:
            case CilCode.Ldelem_I1:
            case CilCode.Ldelem_I2:
            case CilCode.Ldelem_I4:
            case CilCode.Ldelem_I8:
            case CilCode.Ldelem_R4:
            case CilCode.Ldelem_R8:
            case CilCode.Ldelem_Ref:
                block.WithContent(new VmInstruction(VmCode.Ldelem));
                break;
            
            case CilCode.Ldelema:
                block.WithContent(new VmInstruction(VmCode.Ldelema));
                break;
        }
        
        
    }

    public bool Supports(CilInstruction instr)
    {
        return new[]
        {
            CilCode.Stelem,
            CilCode.Stelem_I,
            CilCode.Stelem_I1,
            CilCode.Stelem_I2,
            CilCode.Stelem_I4,
            CilCode.Stelem_I8,
            CilCode.Stelem_R4,
            CilCode.Stelem_R8,
            CilCode.Stelem_Ref,

            CilCode.Ldelem,
            CilCode.Ldelem_I,
            CilCode.Ldelem_I1,
            CilCode.Ldelem_I2,
            CilCode.Ldelem_I4,
            CilCode.Ldelem_I8,
            CilCode.Ldelem_R4,
            CilCode.Ldelem_R8,
            CilCode.Ldelema,
        }.Contains(instr.OpCode.Code);
    }
}