using VirtualGuard.RT.Descriptor;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Chunk;

public class HeaderChunk : IChunk
{
    private VMDescriptor _vmDescriptor;

    public HeaderChunk(VMDescriptor descriptor)
    {
        _vmDescriptor = descriptor;
    }
    public int Length => (sizeof(int) * 10); // this is bs
    
    public void OnOffsetComputed(int offset)
    {
        // do nothing, does not really matter in this case, don't need to update any operands
    }

    public void WriteBytes(BinaryWriter writer)
    {
        
        writer.Write(_vmDescriptor.Watermark.Identifier);
        
        _vmDescriptor.Data.WriteStrings(writer);
        
    }
    
    
}