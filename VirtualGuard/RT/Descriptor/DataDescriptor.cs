using AsmResolver.Patching;
using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Descriptor;

public class DataDescriptor
{
    public DataDescriptor(Random rnd, int debugKey)
    { // debug
        byte[] randomBytes1 = new byte[5];
        byte[] randomBytes2 = new byte[5];
        byte[] randomBytes3 = new byte[5];
        
        rnd.NextBytes(randomBytes1);
        HandlerShifts = randomBytes1;
        
        rnd.NextBytes(randomBytes2);
        ByteShifts = randomBytes2;

        rnd.NextBytes(randomBytes3);
        HeaderRotationFactors = randomBytes3;
        
        InitialHeaderKey = (byte)_rnd.Next(255);

        DebugKey = debugKey;
    }

    public byte InitialHeaderKey;
    public byte[] HeaderRotationFactors;

    public int DebugKey;
    
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