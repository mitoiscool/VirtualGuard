namespace VirtualGuard.RT.Descriptor;

public class DataDescriptor
{
    public DataDescriptor(Random rnd)
    { // debug
        //Reader_IV = (byte)rnd.Next(byte.MaxValue);
        //Reader_Normal_Shift = (byte)rnd.Next(byte.MaxValue);
        //Reader_Handler_Shift = (byte)rnd.Next(byte.MaxValue);
    }
    
    private Dictionary<int, string> _stringMap = new Dictionary<int, string>();

    public string StreamName;
    public string Watermark;

    public byte Reader_IV = 0;
    public byte Reader_Normal_Shift = 0;
    public byte Reader_Handler_Shift = 0;
        
    private static Random _rnd = new Random();
    public uint AddString(string s)
    {
        uint id = (uint)_rnd.Next(int.MaxValue);

        while(_stringMap.ContainsKey((int)id))
            id = (uint)_rnd.Next(int.MaxValue);
        
        _stringMap.Add((int)id, s);

        return id;
    }
    public void WriteStrings(BinaryWriter writer)
    {
        writer.Write(_stringMap.Count);

        foreach (var kvp in _stringMap)
        {
            writer.Write((uint)kvp.Key);
            writer.Write(kvp.Value);
        }
        
    }
    
    
}