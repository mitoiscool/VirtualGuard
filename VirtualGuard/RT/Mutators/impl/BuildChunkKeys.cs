using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

internal class BuildChunkKeys : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        foreach (var chunk in rt.VmChunks)
        {

            foreach (var instr in chunk.Content)
            {
                if (instr.Operand is DynamicStartKeyReference startKeyReference)
                {

                    if (startKeyReference.Chunk == null)
                        throw new Exception("Operand is not expected target chunk.");

                    // set operand to key start

                    instr.Operand = (int)rt.Descriptor.Data.GetStartKey(startKeyReference.Chunk);
                }
                
            }
            
        }
        
        
    }
}