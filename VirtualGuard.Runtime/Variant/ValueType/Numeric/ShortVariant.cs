using System;

namespace VirtualGuard.Runtime.Variant.ValueType.Numeric
{

    public class ShortVariant : NumeralVariant
    {
        private short _value;

        public ShortVariant(short s)
        {
            _value = s;
        }

        public override object GetObject()
        {
            return _value;
        }

        public override void SetFlags(int flag)
        {
            throw new NotImplementedException();
        }

        public override NumeralVariant Add(NumeralVariant addend)
        {
            throw new NotImplementedException();
        }

        public override NumeralVariant Sub(NumeralVariant subtraend)
        {
            throw new NotImplementedException();
        }

        public override NumeralVariant Mul(NumeralVariant factor)
        {
            throw new NotImplementedException();
        }

        public override NumeralVariant Div(NumeralVariant divisor)
        {
            throw new NotImplementedException();
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

        public override NumeralVariant Not()
        {
            throw new NotImplementedException();
        }
    }
}