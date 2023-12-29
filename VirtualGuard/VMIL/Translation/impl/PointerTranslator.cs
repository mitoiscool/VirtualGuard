using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class PointerTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        block.WithContent(new VmInstruction(VmCode.Ldftn, instr.Operand));
    }

    public bool Supports(AstExpression instr)
    {
        return instr.OpCode.Code == CilCode.Ldftn;
    }
}