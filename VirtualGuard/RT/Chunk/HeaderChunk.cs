using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Chunk;

public class HeaderChunk : IChunk
{
    public int Length => (sizeof(int) * 10);
    
    public void OnOffsetComputed(int offset)
    {
        
    }

    public void WriteBytes(BinaryWriter writer)
    {
        
    }
    
    
}