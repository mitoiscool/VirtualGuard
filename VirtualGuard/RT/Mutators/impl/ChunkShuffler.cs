namespace VirtualGuard.RT.Mutators.impl;

public class ChunkShuffler : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if(rt.isDebug)
            return;
        
        rt.GetChunkList().Shuffle();
    }
    
}