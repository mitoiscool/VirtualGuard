using System.Reflection;
using VirtualGuard.Runtime.Dynamic;

namespace VirtualGuard.Runtime;

public class VMData
{
    public static VMData Load()
    {
        if (_inst != null)
            return _inst;

        _inst = new VMData();
        return _inst;
    }

    private static VMData _inst;
    
    private Dictionary<uint, string> _stringMap = new Dictionary<uint, string>();
    private Stream _vmStream;

    private VMData()
    {
        // extract data
        
        // use compression, extract, parse the header and then use 
        
        var asm = Assembly.GetExecutingAssembly();
        using var dataStream = asm.GetManifestResourceStream(Constants.DT_NAME);
        using var reader = new BinaryReader(dataStream);

        // read header
        var watermark = reader.ReadString();
        
        if(watermark != Constants.DT_WATERMARK)
            Routine.Exit(Constants.MSG_INVALID);

        var stringBytes = reader.ReadBytes(Constants.DT_STR_LENGTH);
        var vmBytes = reader.ReadBytes(Constants.DT_VMDATA_LENGTH);
        
        reader.Dispose();
        dataStream.Dispose();

        var ms = new MemoryStream(stringBytes);
        var stringReader = new BinaryReader(ms);

        var stringCount = stringReader.ReadInt32();

        for (int i = 0; i < stringCount; i++)
        {
            _stringMap.Add(stringReader.ReadUInt32(), stringReader.ReadString());
        }

        _vmStream = new MemoryStream(vmBytes);

        ms.Dispose();
        stringReader.Dispose();
    }

    public string GetString(uint key)
    {
        return _stringMap[key];
    }

    public Stream GetVMData()
    {
        return _vmStream;
    }
    
    
}