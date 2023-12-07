namespace VirtualGuard.RT.Descriptor;

public class DataDescriptor
{
    
    private Dictionary<int, string> _stringMap = new Dictionary<int, string>();


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