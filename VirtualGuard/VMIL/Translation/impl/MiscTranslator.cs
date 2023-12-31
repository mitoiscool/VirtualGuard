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
        }
        
    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        return instr.OpCode.Code == CilCode.Dup || instr.OpCode.Code == CilCode.Pop; // lol
    }
}