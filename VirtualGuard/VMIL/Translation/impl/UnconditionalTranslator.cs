using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.Runtime.OpCodes.impl;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class UnconditionalTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        if(node.ConditionalEdges.Count > 0)
            return; // handled in a fallback branch
        
        block.WithContent(
            new VmInstruction(
                VmCode.Ldc_I4,
                node.UnconditionalEdge.Target),
            new VmInstruction(VmCode.Ldc_I4, new DynamicStartKeyReference(node.UnconditionalEdge.Target)),
            new VmInstruction(
                VmCode.Jmp
                )
            );
    }

    public bool Supports(AstExpression instr)
    {
        return instr.OpCode.Code == CilCode.Br;
    }
}