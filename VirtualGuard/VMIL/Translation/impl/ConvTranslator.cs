using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ConvTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Conv_I:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.I));
                break;
            
            case CilCode.Conv_I1:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.I1));
                break;
            
            case CilCode.Conv_I2:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.I2));
                break;
            
            case CilCode.Conv_I4:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.I4));
                break;
            
            case CilCode.Conv_I8:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.I8));
                break;
            
            case CilCode.Conv_U:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.U));
                break;
            
            case CilCode.Conv_U1:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.U1));
                break;
            
            case CilCode.Conv_U2:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.U2));
                break;
            
            case CilCode.Conv_U4:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.U4));
                break;
            
            case CilCode.Conv_U8:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, ctx.Runtime.Descriptor.CorLibTypeDescriptor.U8));
                break;
        }

        block.WithContent(new VmInstruction(VmCode.Conv));
    }

    public bool Supports(AstExpression instr)
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