using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;

namespace VirtualGuard.Transform.IL.impl;

public class BlockConnector : IILTransformer
{
    public void Transform(ControlFlowNode<CilInstruction> input, ControlFlowGraph<CilInstruction> ctx)
    {
        if (input.UnconditionalEdge != null &&
            input.Contents.Instructions.Last().Operand != input.UnconditionalEdge.Target)
            // connect to next block through jmp
            input.Contents.Instructions.Add(
                new CilInstruction(
                    CilOpCodes.Br,
                    input.UnconditionalEdge.Target
                )
            );
    }
}