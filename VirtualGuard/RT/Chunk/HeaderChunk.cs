using VirtualGuard.RT.Descriptor;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Chunk;

public class HeaderChunk : IChunk
{
    private VirtualGuardRT _rt;
    public HeaderChunk(VirtualGuardRT rt)
    {
        _rt = rt;
    }
    public int Length => CalculateLength();
    
    public void OnOffsetComputed(int offset)
    {
        // do nothing, does not really matter in this case, don't need to update any operands
    }

    public void WriteBytes(BinaryWriter writer, VirtualGuardRT rt)
    {
        
        writer.Write(rt.Descriptor.Data.Watermark);
        
        rt.Descriptor.Data.WriteStrings(writer);
    }

    int CalculateLength()
    {
        var ms = new MemoryStream();
        var writer = new BinaryWriter(ms);

        // def a horrible way of doing this
        WriteBytes(writer, _rt);

        return ms.ToArray().Length;
    }
}