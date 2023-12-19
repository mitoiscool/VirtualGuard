using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

public class FinalizeMutations : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        rt.UpdateOffsets(); // shouldn't need to do this again
        
        foreach (var chunk in rt.VmChunks)
        foreach (var instr in chunk.Content)
        {
            if(instr.OpCode != VmCode.Ldc_I4)
                continue;
            
            if(instr.Operand is not MutationOperation mutationOperation)
                continue;

            instr.Operand = mutationOperation.Emulate();
        }

    }


}