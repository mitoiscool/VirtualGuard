namespace VirtualGuard.Runtime.Variant.ValueType.Numeric;

public class ShortVariant : NumeralVariant
    {
        private short _value;

        public ShortVariant(short l)
        {
            _value = l;
        }

        public override object GetObject()
        {
            return _value;
        }

        public override void SetVariantValue(object obj)
        {
            _value = (short)obj;
        }

        public override BaseVariant Clone()
        {
            return new ShortVariant(_value);
        }

        public override NumeralVariant Add(NumeralVariant addend)
        {
            var sum = _value + addend.I2();

            return new ShortVariant((short)sum);
        }

        public override NumeralVariant Sub(NumeralVariant subtraend)
        {
            var diff = _value - subtraend.I2();

            return new ShortVariant((short)diff);
        }

        public override NumeralVariant Mul(NumeralVariant factor)
        {
            var product = _value * factor.I2();

            return new ShortVariant((short)product);
        }

        public override NumeralVariant Div(NumeralVariant divisor)
        {
            var quotient = _value / divisor.I2();

            return new ShortVariant((short)quotient);
        }

        public override NumeralVariant Xor(NumeralVariant xorfactor)
        {
            var result = _value ^ xorfactor.I2();

            return new ShortVariant((short)result);
        }

        public override NumeralVariant Rem(NumeralVariant remfactor)
        {
            var res = _value % remfactor.I2();

            return new ShortVariant((short)res);
        }

        public override NumeralVariant Or(NumeralVariant or)
        {
            var res = _value | or.I2();

            return new ShortVariant((short)res);
        }

        public override NumeralVariant Not()
        {
            return new ShortVariant((short)~_value);
        }

        public override NumeralVariant And(NumeralVariant and)
        {
            return new ShortVariant((short)(_value & and.I2()));
        }
    }