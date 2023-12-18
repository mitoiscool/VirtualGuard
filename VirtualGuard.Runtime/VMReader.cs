using System.Data;
using System.Reflection;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime
{

    public class VMReader : IElement
    {
        /*static VMReader()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream(Constants.DT_NAME);

            _bytes = new byte[stream.Length];

            stream.Read(_bytes, 0, _bytes.Length);


            var reader = new BinaryReader(new MemoryStream(_bytes));

            var watermark = reader.ReadString();
            
            if (watermark != Constants.DT_WATERMARK)
                throw new InvalidDataException(Routines.EncryptDebugMessage("Invalid watermark."));

            var stringCount = reader.ReadInt32();
            var exportCount = reader.ReadInt32();

            for (int i = 0; i < stringCount; i++)
            {
                _stringMap.Add((uint)reader.ReadInt32(), reader.ReadString());
            }

            for (int i = 0; i < exportCount; i++)
            {
                _exportKeyMap.Add(reader.ReadInt32(), reader.ReadByte());
            }
            
        }*/ // old

        static VMReader()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream(Constants.DT_NAME);

            _bytes = new byte[stream.Length];
            int key = Constants.HEADER_IV;

            if (stream.Read(_bytes, 0, _bytes.Length) != stream.Length)
                throw new DataException(Routines.EncryptDebugMessage("Read less bytes than bytes available"));
            
            var ms = new MemoryStream(_bytes);
            
            // read watermark
            var watermarkLength = BitConverter.ToInt16(ReadBytes(ref key, 2, ms), 0);
            var hardcodedHeader = Constants.DT_WATERMARK.ToCharArray();
            
            for (int i = 0; i < watermarkLength; i++)
            {
                //Console.WriteLine(BitConverter.ToChar(ReadBytes(ref key, 2, ms), 0).ToString());
                if (BitConverter.ToChar(ReadBytes(ref key, 2, ms), 0) != hardcodedHeader[i])
                    throw new InvalidDataException(Routines.EncryptDebugMessage("Invalid watermark."));
            }

            var stringCount = BitConverter.ToInt16(ReadBytes(ref key, 2, ms), 0);
            var exportCount = BitConverter.ToInt16(ReadBytes(ref key, 2, ms), 0);

            for (int i = 0; i < stringCount; i++)
            {
                // read int and string
                var stringId = BitConverter.ToInt32(ReadBytes(ref key, 4, ms), 0);
                
                var stringLength = BitConverter.ToInt16(ReadBytes(ref key, 2, ms), 0);
                var chars = new char[stringLength];

                for (int i2 = 0; i2 < stringLength; i2++)
                    chars[i2] = BitConverter.ToChar(ReadBytes(ref key, 2, ms), 0);
                
                // build and add string
                _stringMap.Add((uint)stringId, new string(chars));
            }

            for (int i = 0; i < exportCount; i++)
            {
                // read int and byte
                var offset = BitConverter.ToInt32(ReadBytes(ref key, 4, ms), 0);
                var entryKey = ReadBytes(ref key, 1, ms)[0];
                _exportKeyMap.Add(offset, entryKey);
            }

        }

        
        static byte[] ReadBytes(ref int key, int count, MemoryStream ms)
        {
            byte[] bytes = new byte[count];
            for (int i = 0; i < count; i++)
            {
                byte decByte = (byte)(ms.ReadByte() ^ key);
                key = (byte)((key * Constants.HEADER_ROTATION_FACTOR1) - Constants.HEADER_ROTATION_FACTOR2 + (decByte ^ Constants.HEADER_ROTATION_FACTOR3));
                bytes[i] = decByte;
            }

            return bytes;
        }
        
        
        private static readonly byte[] _bytes;
        private static readonly Dictionary<uint, string> _stringMap = new Dictionary<uint, string>();
        private static readonly Dictionary<int, byte> _exportKeyMap = new Dictionary<int, byte>();

        public VMReader()
        {
            _memoryStream = new MemoryStream(_bytes);
        }

        private readonly MemoryStream _memoryStream;
        private byte _key;

        public void SetKey(byte i)
        {
            _key = i;
        }

        public UIntVariant ReadHandler()
        {
            var b = (byte)_memoryStream.ReadByte();

            var dec = (byte)(b ^ _key);
            
            //Console.WriteLine("dec {0} enc {1} key {2}", dec, b, _key);
            
            _key = (byte)((_key * Constants.HANDLER_ROT1) + dec + (Constants.HANDLER_ROT2 >> (Constants.HANDLER_ROT3 ^ Constants.HANDLER_ROT4)) * Constants.HANDLER_ROT5);

            return new UIntVariant(dec);
            //return new ByteVariant((byte)(b ^ _key));
        }

        public UIntVariant ReadByte()
        {
            return new UIntVariant(ReadByteInternal());
        }

        public IntVariant ReadShort()
        {
            return new IntVariant(BitConverter.ToInt16(ReadPrimitive(2), 0));
        }

        public IntVariant ReadInt()
        {
            return new IntVariant(BitConverter.ToInt32(ReadPrimitive(4), 0));
        }

        public LongVariant ReadLong()
        {
            return new LongVariant(BitConverter.ToInt64(ReadPrimitive(8), 0));
        }

        public BaseVariant ReadString(BaseVariant id)
        {

            if (id.U4() == 0)
                return new NullVariant();

            // read from vm data dict
            return new StringVariant(_stringMap[id.U4()]);
        }

        public static byte GetEntryKey(int loc)
        {
            return _exportKeyMap[loc];
        }

        public void SetValue(int i)
        {
            //Console.WriteLine("set reader to loc " + i);
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
            try
            {
                var b = _memoryStream.ReadByte();

                //_key ;

                b ^= _key;
                
                // key = (operandShifts[0] ^ operandShifts[1]) >> (operandShifts[2] << (operandShifts[3] * key)) + (operandShifts[4] * b);

                _key = (byte)((Constants.BYTE_ROT1 ^ Constants.BYTE_ROT2) - (b + (Constants.BYTE_ROT3 * _key)) ^ (Constants.BYTE_ROT4 + Constants.BYTE_ROT5));
                
                return (byte)b;
            }
            catch
            {
                throw new InvalidDataException(Routines.EncryptDebugMessage("Error reading byte."));
            }
        }

    }
}