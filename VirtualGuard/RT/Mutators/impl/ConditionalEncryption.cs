using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

internal class ConditionalEncryption : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        foreach (var chunk in rt.VmChunks)
        foreach (var instr in chunk.Content)
        {
            if(instr.Operand is not DynamicStartKeyReference dynamicStartKeyReference)
                continue;
            
            if(!dynamicStartKeyReference.IsConditional)
                continue;
            
            
        }
        
        
        
    }
}