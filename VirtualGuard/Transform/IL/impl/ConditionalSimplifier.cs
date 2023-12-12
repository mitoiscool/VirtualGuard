using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;

namespace VirtualGuard.Transform.IL.impl;

public class ConditionalSimplifier : IILTransformer
{
    public void Transform(ControlFlowNode<CilInstruction> node, ControlFlowGraph<CilInstruction> ctx)
    {
        var oldInstrs = node.Contents.Instructions.ToArray();

        node.Contents.Instructions.Clear();
        
        
        foreach (var oldInstr in oldInstrs)
        {

            if (Simplify(oldInstr, out CilInstruction[] instrs))
            {
                foreach (var instr in instrs)
                {
                    node.Contents.Instructions.Add(instr);
                }
                
            }
            else
            {
                node.Contents.Instructions.Add(oldInstr);
            }
        }
    }
    
    bool Simplify(CilInstruction instr, out CilInstruction[] instrs)
    {
        var instructions = new List<CilInstruction>();

        switch (instr.OpCode.Code)
        {
            case CilCode.Beq:
                instructions.Add(new CilInstruction(CilOpCodes.Ceq));
                instructions.Add(new CilInstruction(CilOpCodes.Brtrue, instr.Operand));
                break;
            
            case CilCode.Bne_Un:
                instructions.Add(new CilInstruction(CilOpCodes.Ceq));
                instructions.Add(new CilInstruction(CilOpCodes.Brfalse, instr.Operand));
                break;
            
            case CilCode.Bge:
            case CilCode.Bge_Un:
                instructions.Add(new CilInstruction(CilOpCodes.Clt));
                instructions.Add(new CilInstruction(CilOpCodes.Brfalse, instr.Operand));
                break;

            case CilCode.Bgt:
            case CilCode.Bgt_Un:
                instructions.Add(new CilInstruction(CilOpCodes.Cgt));
                instructions.Add(new CilInstruction(CilOpCodes.Brtrue, instr.Operand));
                break;
            
            case CilCode.Ble:
            case CilCode.Ble_Un:
                instructions.Add(new CilInstruction(CilOpCodes.Cgt));
                instructions.Add(new CilInstruction(CilOpCodes.Brfalse, instr.Operand));
                    break;
            
            case CilCode.Blt:
            case CilCode.Blt_Un:
                instructions.Add(new CilInstruction(CilOpCodes.Clt));
                instructions.Add(new CilInstruction(CilOpCodes.Brtrue, instr.Operand));
                    break;
            
            default:
                instrs = Array.Empty<CilInstruction>();
                return false;
        }

        throw new InvalidOperationException();
    }

}