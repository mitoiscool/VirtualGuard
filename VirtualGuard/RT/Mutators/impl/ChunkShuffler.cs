namespace VirtualGuard.RT.Mutators.impl;

public class ChunkShuffler : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        return;
        
        
        if(rt.isDebug)
            return;
        
        rt.GetChunkList().Shuffle();
        
        rt.UpdateOffsets();
    }
    
}