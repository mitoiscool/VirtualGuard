using System.Text;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.Variant.ValueType
{

    public class StringVariant : BaseVariant
    {
        private string _str;

        public StringVariant(string str)
        {
            _str = str;
        }

        public override object GetObject()
        {
            return _str;
        }

        public override void SetVariantValue(object obj)
        {
            _str = (string)obj;
        }

        public override BaseVariant Clone()
        {
            return new StringVariant(_str);
        }

        public override BaseVariant Hash()
        {
            byte[] buffer = Encoding.ASCII.GetBytes(_str);
        
            uint[] table = new uint[256];

            for (uint i = 0; i < 256; i++)
            {
                uint entry = i;
                for (int j = 0; j < 8; j++)
                {
                    entry = (entry & 1) == 1
                        ? (entry >> 1) ^ Constants.SPolynomial
                        : entry >> 1;

                    // Additional XOR to make the algorithm more unique
                    entry ^= (entry >> 12) ^ (entry >> 24);
                }

                table[i] = entry;
            }

            uint hash = Constants.SSeed;

            for (int i = 0; i < buffer.Length; i++)
            {
                hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
            }

            return new LongVariant((long)(~hash ^ Constants.SXorMask));
        }
        
    }
}