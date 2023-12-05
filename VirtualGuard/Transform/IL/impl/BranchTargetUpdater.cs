using System.Data;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;

namespace VirtualGuard.Transform.IL.impl;

public class BranchTargetUpdater : IILTransformer
{
    public void Transform(ControlFlowNode<CilInstruction> node, ControlFlowGraph<CilInstruction> ctx)
    {
        var headers = ctx.Nodes.ToDictionary(x => x.Contents.Header);


        foreach (var instr in node.Contents.Instructions)
        {
            if (instr.Operand is CilInstructionLabel i)
            {
                var newOperand = headers.Single(
                    x => x.Key.Equals(i.Instruction)
                );

                instr.Operand = newOperand.Value;
            }

            if (instr.Operand is CilInstructionLabel[] ia)
            {
                var newBranchTable = new List<ControlFlowNode<CilInstruction>>();

                foreach (var branch in ia)
                    newBranchTable.Add(
                        headers.Single(
                            x => x.Key.Equals(branch.Instruction)
                        ).Value
                    );

                instr.Operand = newBranchTable.ToArray();
            }
        }
        
    }
}