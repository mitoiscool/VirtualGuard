using System.Reflection;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime;

public class VMReader : IElement
{
    static VMReader()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var stream = assembly.GetManifestResourceStream(Constants.RSRC_NAME);

        _bytes = new byte[stream.Length];

        stream.Read(_bytes, 0, _bytes.Length);

        var reader = new BinaryReader(stream);

        var watermark = reader.ReadString();

        if (watermark != Constants.DT_WATERMARK)
            throw new InvalidDataException();

        var stringCount = reader.ReadInt32();

        for (int i = 0; i < stringCount; i++)
        {
            _stringMap.Add((uint)reader.ReadInt32(), reader.ReadString());
        }
        
    }

    private static byte[] _bytes;
    private static Dictionary<uint, string> _stringMap = new Dictionary<uint, string>();
    
    public VMReader()
    {
        _memoryStream = new MemoryStream(_bytes);
        _key = Constants.RD_IV;
    }

    private MemoryStream _memoryStream;
    private int _key;

    public ByteVariant ReadHandler()
    {
        var b = _memoryStream.ReadByte();

        _key += Constants.RD_HANDLER_ROT;
        
        return new ByteVariant((byte)(b ^ _key));
    }

    public ByteVariant ReadByte()
    {
        return new ByteVariant(ReadByteInternal());
    }
    
    public ShortVariant ReadShort()
    {
        return new ShortVariant(BitConverter.ToInt16(ReadPrimitive(2), 0));
    }

    public IntVariant ReadInt()
    {
        return new IntVariant(BitConverter.ToInt16(ReadPrimitive(4), 0));
    }

    public LongVariant ReadLong()
    {
        return new LongVariant(BitConverter.ToInt64(ReadPrimitive(8), 0));
    }

    public BaseVariant ReadString(BaseVariant id)
    {
        
        if (id.U4() == 0)
            return new NullVariant();
        
        // read from vmdata dict
        return new StringVariant(_stringMap[id.U4()]);
    }
    
    public void SetValue(int i)
    {
        _memoryStream.Seek(i, SeekOrigin.Begin);
    }

    public int GetValue()
    {
        return (int)_memoryStream.Position;
    }
    
    private byte[] ReadPrimitive(int length)
    {
        byte[] buffer = new byte[length];
        for (int i = 0; i < length; i++)
        {
            buffer[i] = ReadByteInternal();
        }
        return buffer;
    }
    
    private byte ReadByteInternal()
    {
        var b = _memoryStream.ReadByte();

        _key += Constants.RD_BYTE_ROT;

        return (byte)(b ^ _key);
    }
    
}