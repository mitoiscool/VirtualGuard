using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.ValueType;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime;

public class VMReader : IElement
{
    public VMReader(VMData data)
    {
        _data = data;
    }

    private VMData _data;
    private int _key;

    public ByteVariant ReadHandler()
    {
        var b = _data.GetVMData().ReadByte();

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

    public StringVariant ReadString(BaseVariant id)
    {
        // read from vmdata dict
        return new StringVariant(_data.GetString(id.U4()));
    }

    public void SetValue(int i)
    {
        _data.GetVMData().Seek(i, SeekOrigin.Begin);
    }

    public int GetValue()
    {
        return (int)_data.GetVMData().Position;
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
        var b = _data.GetVMData().ReadByte();

        _key += Constants.RD_BYTE_ROT;

        return (byte)(b ^ _key);
    }
    
}