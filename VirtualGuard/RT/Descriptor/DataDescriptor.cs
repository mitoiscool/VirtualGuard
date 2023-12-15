using VirtualGuard.RT.Chunk;

namespace VirtualGuard.RT.Descriptor;

public class DataDescriptor
{
    public DataDescriptor(Random rnd)
    { // debug
        byte[] randomBytes1 = new byte[10];
        
        rnd.NextBytes(randomBytes1);
        HandlerShifts = randomBytes1;
        
        rnd.NextBytes(randomBytes1);
        ByteShifts = randomBytes1;
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
    
    public byte[] ByteShifts;
    public byte[] HandlerShifts;
        
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