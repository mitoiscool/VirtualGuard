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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override NumeralVariant Rem(NumeralVariant remfactor)
        {
            throw new NotImplementedException();
        }

        public override NumeralVariant Or(NumeralVariant or)
        {
            throw new NotImplementedException();
        }

        public override NumeralVariant Not(NumeralVariant not)
        {
            throw new NotImplementedException();
        }
    }
}