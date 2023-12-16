using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ConvTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Conv_I:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.IntPtr.ToTypeDefOrRef()));
                break;
            
            case CilCode.Conv_I1:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.SByte.ToTypeDefOrRef()));
                break;
            
            case CilCode.Conv_I2:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.Int16.ToTypeDefOrRef()));
                break;
            
            case CilCode.Conv_I4:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.Int32.ToTypeDefOrRef()));
                break;
            
            case CilCode.Conv_I8:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.Int64.ToTypeDefOrRef()));
                break;
            
            case CilCode.Conv_U:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.UIntPtr.ToTypeDefOrRef()));
                break;
            
            case CilCode.Conv_U1:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.Byte.ToTypeDefOrRef()));
                break;
            
            case CilCode.Conv_U2:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.UInt16.ToTypeDefOrRef()));
                break;
            
            case CilCode.Conv_U4:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.UInt32.ToTypeDefOrRef()));
                break;
            
            case CilCode.Conv_U8:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Module.CorLibTypeFactory.UInt64.ToTypeDefOrRef()));
                break;
        }

        block.WithContent(new VmInstruction(VmCode.Conv));
    }

    public bool Supports(CilInstruction instr)
    {
        return new[]
        {
            CilCode.Conv_I,
            CilCode.Conv_I1,
            CilCode.Conv_I2,
            CilCode.Conv_I4,
            CilCode.Conv_I8,
            CilCode.Conv_U,
            CilCode.Conv_U1,
            CilCode.Conv_U2,
            CilCode.Conv_U4,
            CilCode.Conv_U8
        }.Contains(instr.OpCode.Code);
    }
}