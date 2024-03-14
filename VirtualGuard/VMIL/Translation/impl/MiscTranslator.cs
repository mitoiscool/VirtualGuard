using System.Reflection;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class MiscTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Dup:
                block.WithContent(new VmInstruction(VmCode.Dup));
                break;
            
            case CilCode.Pop:
                block.WithContent(new VmInstruction(VmCode.Pop));
                break;
            
            case CilCode.Castclass:
                block.WithContent(new VmInstruction(VmCode.Castclass, instr.Operand));
                break;
            
            case CilCode.Box:
                block.WithContent(new VmInstruction(VmCode.Box, instr.Operand));
                break;
            
            case CilCode.Unbox_Any:
                block.WithContent(new VmInstruction(VmCode.Unboxany, instr.Operand));
                break;
            
            case CilCode.Sizeof:
                block.WithContent(new VmInstruction(VmCode.Sizeof, instr.Operand));
                break;
            
            case CilCode.Unbox:
                block.WithContent(new VmInstruction(VmCode.Unbox, instr.Operand));
                break;
        }
        
    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        return instr.OpCode.Code == CilCode.Dup || instr.OpCode.Code == CilCode.Pop || instr.OpCode.Code == CilCode.Castclass || instr.OpCode.Code == CilCode.Box || instr.OpCode.Code == CilCode.Unbox_Any || instr.OpCode.Code == CilCode.Sizeof || instr.OpCode.Code == CilCode.Unbox; // lol
    }
}