using System;

namespace VirtualGuard.Runtime.Variant.ValueType.Numeric
{

    public class ByteVariant : NumeralVariant
    {
        private byte _b;

        public ByteVariant(byte b)
        {
            _b = b;
        }

        public override object GetObject()
        {
            return _b;
        }

        public override void SetValue(object obj)
        {
            _b = (byte)obj;
        }

        public override BaseVariant Clone()
        {
            return new ByteVariant(_b);
        }

        public override void SetFlags(int flag)
        {
            throw new NotImplementedException();
        }

        public override NumeralVariant Add(NumeralVariant addend)
        {
            var sum = this.U1() + addend.U1();

            return new ByteVariant((byte)sum);
        }

        public override NumeralVariant Sub(NumeralVariant subtraend)
        {
            var sum = this.U1() - subtraend.U1();

            return new ByteVariant((byte)sum);
        }

        public override NumeralVariant Mul(NumeralVariant factor)
        {
            var sum = this.U1() * factor.U1();

            return new ByteVariant((byte)sum);
        }

        public override NumeralVariant Div(NumeralVariant divisor)
        {
            var sum = this.U1() / divisor.U1();

            return new ByteVariant((byte)sum);
        }

        public override NumeralVariant Xor(NumeralVariant xorfactor)
        {
            return new ByteVariant((byte)(_b ^ xorfactor.U1()));
        }

        public override NumeralVariant Rem(NumeralVariant remfactor)
        {
            return new ByteVariant((byte)(_b % remfactor.U1()));
        }
        public override NumeralVariant Or(NumeralVariant or)
        {
            return new ByteVariant((byte)(_b | or.U1()));
        }

        public override NumeralVariant Not()
        {
            return new ByteVariant((byte)~_b);
        }
    }
}