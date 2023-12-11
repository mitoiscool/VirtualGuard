using VirtualGuard.RT.Descriptor;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Chunk;

public class HeaderChunk : IChunk
{
    public int Length => (sizeof(int) * 10); // this is bs
    
    public void OnOffsetComputed(int offset)
    {
        // do nothing, does not really matter in this case, don't need to update any operands
    }

    public void WriteBytes(BinaryWriter writer, VirtualGuardRT rt)
    {
        
        writer.Write(rt.Descriptor.Data.Watermark);
        
        rt.Descriptor.Data.WriteStrings(writer);
    }
    
    
}