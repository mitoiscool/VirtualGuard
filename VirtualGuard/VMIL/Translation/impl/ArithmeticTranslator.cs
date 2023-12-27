using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ArithmeticTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Add:
                block.WithContent(new VmInstruction(VmCode.Add));
                break;
            
            case CilCode.Sub:
                block.WithContent(new VmInstruction(VmCode.Sub));
                break;
            
            case CilCode.Mul:
                block.WithContent(new VmInstruction(VmCode.Mul));
                break;
            
            case CilCode.Div:
                block.WithContent(new VmInstruction(VmCode.Div));
                break;
            
            case CilCode.Rem:
                block.WithContent(new VmInstruction(VmCode.Rem));
                break;
        }
    }

    public bool Supports(AstExpression instr)
    {
        var supported = new CilOpCode[]
        {
            CilOpCodes.Add,
            CilOpCodes.Sub,
            CilOpCodes.Mul,
            CilOpCodes.Div,
            
            CilOpCodes.Rem
        };

        return supported.Contains(instr.OpCode);
    }
    
    
    
    
}