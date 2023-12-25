using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ArrayTranslator : ITranslator
{
    public void Translate(AstExpression instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
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
            case CilCode.Ldelem_U1:
            case CilCode.Ldelem_U2:
            case CilCode.Ldelem_U4:
            case CilCode.Ldelem_R4:
            case CilCode.Ldelem_R8:
            case CilCode.Ldelem_Ref:
                block.WithContent(new VmInstruction(VmCode.Ldelem));
                break;
            
            // this might be necessary, not sure yet
                block.WithContent(new VmInstruction(VmCode.Ldelem),
                    new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.U2),
                    new VmInstruction(VmCode.Conv));
                break;
            
            case CilCode.Ldelema:
                block.WithContent(new VmInstruction(VmCode.Ldelema));
                break;
            
            case CilCode.Newarr:
                block.WithContent(new VmInstruction(VmCode.Newarr, instr.Operand));
                break;
            
            case CilCode.Ldlen:
                block.WithContent(new VmInstruction(VmCode.Ldlen));
                break;
        }
        
        
    }

    public bool Supports(AstExpression instr)
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
            CilCode.Ldelem_U1,
            CilCode.Ldelem_U2,
            CilCode.Ldelem_U4,
            CilCode.Ldelem_R4,
            CilCode.Ldelem_R8,
            CilCode.Ldelema,
            
            CilCode.Newarr,
            CilCode.Ldlen
        }.Contains(instr.OpCode.Code);
    }
}