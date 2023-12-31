namespace VirtualGuard.RT.Mutators.impl;

internal class ChunkShuffler : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if(rt.isDebug)
            return;
        
        rt.GetChunkList().Shuffle();
        
        rt.UpdateOffsets();
    }
    
}