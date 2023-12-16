namespace VirtualGuard.RT.Mutators.impl;

public class ChunkShuffler : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if(rt.isDebug)
            return;
        
        Shuffle(rt.GetChunkList());
    }
    
    void Shuffle<T>(List<T> list)
    {
        Random rng = new Random();
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}