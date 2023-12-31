namespace VirtualGuard.RT.Mutators.impl;

internal class EncodeStrings : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        
        
        foreach (var instr in rt.VmChunks.SelectMany(x => x.Content))
        {
            if (instr.Operand is string str)
            {
                instr.Operand = rt.Descriptor.Data.AddString(str);
            }
            
            
        }
        
        
        
    }
}