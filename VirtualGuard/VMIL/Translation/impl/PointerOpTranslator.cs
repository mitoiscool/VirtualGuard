using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class PointerOpTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        throw new NotImplementedException();
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
            
            default:
                return false;
        }
        
        
    }
}