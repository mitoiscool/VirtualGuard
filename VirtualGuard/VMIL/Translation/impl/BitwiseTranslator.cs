using System.Reflection;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class BitwiseTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.And:
                block.WithContent(new VmInstruction(VmCode.And));
                break;
            
            case CilCode.Xor:
                block.WithContent(new VmInstruction(VmCode.Xor));
                break;
            
            case CilCode.Or:
                block.WithContent(new VmInstruction(VmCode.Or));
                break;
            
            case CilCode.Not:
                block.WithContent(new VmInstruction(VmCode.Not));
                break;
            
        }
    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Xor,
            CilCode.Or,
            CilCode.And,
            CilCode.Not
        }.Contains(instr.OpCode.Code);
    }
}