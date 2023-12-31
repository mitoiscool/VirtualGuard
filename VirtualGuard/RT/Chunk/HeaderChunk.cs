using System.Data;
using VirtualGuard.RT.Descriptor;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Chunk;

internal class HeaderChunk : IChunk
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
        // we need to encrypt
        
        // build bytes ( is this the best way? )

        var byteList = new List<byte>();
        
        var strings = rt.Descriptor.Data.GetStrings();
        var exports = rt.Descriptor.Data.GetExports();
        
        byteList.AddRange(GetBytes(rt.Descriptor.Data.Watermark));
        
        byteList.AddRange(GetBytes((short)strings.Count));
        byteList.AddRange(GetBytes((short)exports.Count));

        foreach (var s in strings)
        {
            byteList.AddRange(GetBytes(s.Key));
            byteList.AddRange(GetBytes(s.Value));
        }

        foreach (var export in exports)
        {
            byteList.AddRange(GetBytes(export.Key.Offset));
            byteList.AddRange(GetBytes(export.Value));
        }
        
        // encrypt
        var key = rt.Descriptor.Data.InitialHeaderKey;
        var finalizedBytes = byteList.ToArray();
        var keys = rt.Descriptor.Data.HeaderRotationFactors;

        for (int i = 0; i < finalizedBytes.Length; i++)
        {
            var og = finalizedBytes[i];
            finalizedBytes[i] = (byte)(og ^ key);
            
            //Console.WriteLine("dec: {0} key: {1}", og, key);

            key = (byte)((key * keys[0]) - keys[1] + (og ^ keys[2]));
            // key = (byte)((key * Constants.HEADER_ROTATION_FACTOR1) - Constants.HEADER_ROTATION_FACTOR2 + (decByte ^ Constants.HEADER_ROTATION_FACTOR3));
        }

        writer.Write(finalizedBytes);
    }

    byte[] GetBytes(object obj)
    {
        if (obj is string s)
        {
            var bytes = new List<byte>();
            var chars = s.ToCharArray();
            
            bytes.AddRange(BitConverter.GetBytes((short)chars.Length));

            foreach (var c in chars)
            {
                bytes.AddRange(BitConverter.GetBytes(c));
            }

            return bytes.ToArray();
        }

        if (obj is int i)
            return BitConverter.GetBytes(i);

        if (obj is byte b)
            return new[] { b };

        if (obj is short sh)
            return BitConverter.GetBytes(sh);
        
        throw new DataException(obj.GetType().Name);
    }

    int CalculateLength()
    {
        int length = 0;

        length += GetBytes(_rt.Descriptor.Data.Watermark).Length;

        length += (sizeof(short) * 2); // sizes for both the sizes of exports and strings

        foreach (var str in _rt.Descriptor.Data.GetStrings())
        {
            length += sizeof(int);
            length += GetBytes(str.Value).Length;
        }

        // just get exports and multiply by their size
        
        length += (sizeof(int) + sizeof(byte)) *_rt.Descriptor.Data.GetExports().Count;

        return length;
    }
    
    
}