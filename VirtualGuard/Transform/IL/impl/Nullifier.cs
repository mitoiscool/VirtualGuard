using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;

namespace VirtualGuard.Transform.IL.impl;

public class Nullifier : IILTransformer
{
    public void Transform(ControlFlowNode<CilInstruction> node, ControlFlowGraph<CilInstruction> ctx)
    {
        var instructions = node.Contents.Instructions;
        
        for (int i = instructions.Count - 1; i >= 0; i--)
        {
            var instruction = instructions[i];
            if (instruction.OpCode.Code == CilCode.Nop)
            {
                instructions.RemoveAt(i);
            }
        }
    }
}