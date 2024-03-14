using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class PointerOpTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Stind_I:
                block.WithContent(new VmInstruction(VmCode.Stind, ctx.Module.CorLibTypeFactory.IntPtr.ToTypeDefOrRef()));
                break;
            case CilCode.Stind_I1:
                block.WithContent(new VmInstruction(VmCode.Stind, ctx.Module.CorLibTypeFactory.SByte.ToTypeDefOrRef()));
                break;
            case CilCode.Stind_I2:
                block.WithContent(new VmInstruction(VmCode.Stind, ctx.Module.CorLibTypeFactory.Int16.ToTypeDefOrRef()));
                break;
            case CilCode.Stind_I4:
                block.WithContent(new VmInstruction(VmCode.Stind, ctx.Module.CorLibTypeFactory.Int32.ToTypeDefOrRef()));
                break;
            case CilCode.Stind_I8:
                block.WithContent(new VmInstruction(VmCode.Stind, ctx.Module.CorLibTypeFactory.Int64.ToTypeDefOrRef()));
                break;
            case CilCode.Stind_R4:
                block.WithContent(new VmInstruction(VmCode.Stind, ctx.Module.CorLibTypeFactory.Single.ToTypeDefOrRef()));
                break;
            case CilCode.Stind_R8:
                block.WithContent(new VmInstruction(VmCode.Stind, ctx.Module.CorLibTypeFactory.Double.ToTypeDefOrRef()));
                break;
            case CilCode.Stind_Ref:
                block.WithContent(new VmInstruction(VmCode.Stind, ctx.Module.CorLibTypeFactory.Object.ToTypeDefOrRef()));
                break;
            
            case CilCode.Ldind_I:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.IntPtr.ToTypeDefOrRef()));
                break;
            case CilCode.Ldind_I1:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.SByte.ToTypeDefOrRef()));
                break;
            case CilCode.Ldind_I2:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.Int16.ToTypeDefOrRef()));
                break;
            case CilCode.Ldind_I4:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.Int32.ToTypeDefOrRef()));
                break;
            case CilCode.Ldind_I8:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.Int64.ToTypeDefOrRef()));
                break;
            
            case CilCode.Ldind_U1:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.Byte.ToTypeDefOrRef()));
                break;
            case CilCode.Ldind_U2:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.UInt16.ToTypeDefOrRef()));
                break;
            case CilCode.Ldind_U4:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.UInt32.ToTypeDefOrRef()));
                break;
            
            case CilCode.Ldind_R4:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.Single.ToTypeDefOrRef()));
                break;
            case CilCode.Ldind_R8:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.Double.ToTypeDefOrRef()));
                break;
            
            case CilCode.Ldind_Ref:
                block.WithContent(new VmInstruction(VmCode.Ldind, ctx.Module.CorLibTypeFactory.Object.ToTypeDefOrRef()));
                break;
            
            case CilCode.Stobj:
                block.WithContent(new VmInstruction(VmCode.Stind,
                    ctx.Module.CorLibTypeFactory.Object.ToTypeDefOrRef()));
                break;
            
            case CilCode.Ldobj:
                block.WithContent(new VmInstruction(VmCode.Ldind,
                    ctx.Module.CorLibTypeFactory.Object.ToTypeDefOrRef()));
                break;
            
            case CilCode.Ldftn:
                block.WithContent(new VmInstruction(VmCode.Ldftn, instr.Operand));
                break;
        }
        
    }

    public bool Supports(AstExpression instr)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Stind_I:
            case CilCode.Stind_I1:
            case CilCode.Stind_I2:
            case CilCode.Stind_I4:
            case CilCode.Stind_I8:
                return true;
            
            case CilCode.Stind_R4:
            case CilCode.Stind_R8:
                
            case CilCode.Stind_Ref:
                return true;
            
            case CilCode.Ldind_I:
            case CilCode.Ldind_I1:
            case CilCode.Ldind_I2:
            case CilCode.Ldind_I4:
            case CilCode.Ldind_I8:
            case CilCode.Ldind_U2:
            case CilCode.Ldind_U4:
            case CilCode.Ldind_Ref:
            case CilCode.Ldind_R4:
            case CilCode.Ldind_R8:
                return true;
            
            case CilCode.Stobj:
            case CilCode.Ldobj:
                return true;
            
            case CilCode.Ldftn:
                return true;
            
            default:
                return false;
        }
        
        
    }
}