using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

public class MarkChunks : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        foreach (var chunk in rt.VmChunks)
        {
            rt.Descriptor.Data.BuildStartKey(chunk);
        }

        foreach (var chunk in rt.VmChunks)
        {

            foreach (var instr in chunk.Content)
            {
                if (instr.OpCode == VmCode.Jmp || instr.OpCode == VmCode.Jz)
                {


                    // locate instruction before to find the target
                    var index = chunk.Content.IndexOf(instr) - 1;

                    var targetOperand = chunk.Content[index].Operand;

                    if (targetOperand is not VmChunk targetChunk)
                        throw new Exception("Operand is not expected target chunk.");

                    // set operand to key start

                    instr.Operand = (int)rt.Descriptor.Data.GetStartKey(targetChunk);
                }
                
            }
            
        }
        
        
    }
}