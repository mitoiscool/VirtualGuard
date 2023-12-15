using VirtualGuard.RT.Chunk;

namespace VirtualGuard.RT.Descriptor;

public class DataDescriptor
{
    public DataDescriptor(Random rnd)
    { // debug
        
        Reader_IV = (byte)rnd.Next(byte.MaxValue);
        Reader_Normal_Shift = (byte)rnd.Next(byte.MaxValue);
        Reader_Handler_Shift = (byte)rnd.Next(byte.MaxValue);
        
    }
    
    private Dictionary<int, string> _stringMap = new Dictionary<int, string>();

    private Dictionary<VmChunk, byte> _chunkKeyMap = new Dictionary<VmChunk, byte>();

    public byte GetStartKey(VmChunk chunk)
    {
        
        return _chunkKeyMap[chunk];
    }

    public void BuildStartKey(VmChunk chunk)
    {
        var startKey = (byte)_rnd.Next(255);
        _chunkKeyMap.Add(chunk, startKey);
    }

    public Dictionary<VmChunk, byte> GetExports()
    {
        return _chunkKeyMap;
    }

    public string StreamName;
    public string Watermark;

    public byte Reader_IV = 0;
    public byte Reader_Normal_Shift = 0;
    public byte Reader_Handler_Shift = 0;
        
    private static Random _rnd = new Random();
    public int AddString(string s)
    {
        int id = _rnd.Next(int.MaxValue);

        while(_stringMap.ContainsKey(id))
            id = _rnd.Next(int.MaxValue);
        
        _stringMap.Add(id, s);

        return id;
    }

    public Dictionary<int, string> GetStrings()
    {
        return _stringMap;
    }
    
}