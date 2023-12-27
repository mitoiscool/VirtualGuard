using System.Diagnostics;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;

namespace VirtualGuard.Transform.IL.impl;

public class BranchUpdater : IILTransformer
{
    public void Transform(ControlFlowNode<CilInstruction> node, ControlFlowGraph<CilInstruction> ctx)
    {


        bool condition = true;

        foreach (var instr in node.Contents.Instructions)
        {

            if (instr.OpCode.Code == CilCode.Br || instr.OpCode.Code == CilCode.Leave) // pray it occurs after cond
            {
                instr.Operand = node.UnconditionalEdge.Target;
            }
            
            if (instr.OpCode.Code == CilCode.Brtrue || instr.OpCode.Code == CilCode.Brfalse) // contains brtrue, use cond edge
            {
                instr.Operand = node.ConditionalEdges.First().Target;
            }

            if (instr.OpCode.Code == CilCode.Switch)
                instr.Operand = node.ConditionalEdges.Select(x => x.Target).ToArray();

        }
    }
}