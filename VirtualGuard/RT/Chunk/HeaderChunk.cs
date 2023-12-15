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
        var strings = rt.Descriptor.Data.GetStrings();
        var exports = rt.Descriptor.Data.GetExports();
        
        writer.Write(rt.Descriptor.Data.Watermark);
        
        writer.Write(strings.Count);
        writer.Write(exports.Count);

        foreach (var s in strings)
        {
            writer.Write(s.Key);
            writer.Write(s.Value);
        }

        foreach (var export in exports)
        {
            writer.Write(export.Key.Offset);
            writer.Write(export.Value);
        }
        
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