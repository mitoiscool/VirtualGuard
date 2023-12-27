using System.Diagnostics;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;

namespace VirtualGuard.Transform.IL.impl;

public class BranchUpdater : IILTransformer
{
    public void Transform(ControlFlowNode<CilInstruction> node, ControlFlowGraph<CilInstruction> ctx)
    {
        /*
        
        bool condition = true;
        
        foreach (var instr in node.Contents.Instructions)
        {

            if (instr.OpCode.Code == CilCode.Br || instr.OpCode.Code == CilCode.Leave) // pray it occurs after cond
            {
                if (condition == false)
                { // if cond is false we swapped the sides
                    instr.Operand = node.ConditionalEdges.First().Target;
                }
                else
                {
                    instr.Operand = node.UnconditionalEdge.Target;
                }
            }
            

            if (instr.OpCode.Code == CilCode.Brtrue || instr) // contains brtrue, use cond edge
            {
                instr.Operand = node.ConditionalEdges.First().Target;
                condition = true;
                
                Debug.Assert(node.ConditionalEdges.Count == 1); // unsure
            }

            if (instr.OpCode.Code == CilCode.Brfalse)
            {
                instr.Operand = node.UnconditionalEdge.Target;
                condition = false;
            }

            
            if (instr.OpCode.Code == CilCode.Switch)
                instr.Operand = node.ConditionalEdges.Select(x => x.Target).ToArray();
            */
        }
    
}