using System;
using System.Runtime.InteropServices;
using VirtualGuard.Runtime.Dynamic;

namespace VirtualGuard.Runtime.Variant.ValueType.Numeric
{

    public class LongVariant : NumeralVariant
    {
        private long _value;

        public LongVariant(long l)
        {
            _value = l;
        }

        public override object GetObject()
        {
            return _value;
        }

        public override void SetVariantValue(object obj)
        {
            _value = (long)obj;
        }

        public override BaseVariant Clone()
        {
            return new LongVariant(_value);
        }
        
        public override NumeralVariant Add(NumeralVariant addend)
        {
            var sum = _value + addend.I8();

            return new LongVariant(sum);
        }

        public override NumeralVariant Sub(NumeralVariant subtraend)
        {
            var diff = _value - subtraend.I8();

            return new LongVariant(diff);
        }

        public override NumeralVariant Mul(NumeralVariant factor)
        {
            var product = _value * factor.I8();

            return new LongVariant(product);
        }

        public override NumeralVariant Div(NumeralVariant divisor)
        {
            var quotient = _value / divisor.I8();

            return new LongVariant(quotient);
        }

        public override NumeralVariant Xor(NumeralVariant xorfactor)
        {
            var result = _value ^ xorfactor.I8();

            return new LongVariant(result);
        }

        public override NumeralVariant Rem(NumeralVariant remfactor)
        {
            var res = _value % remfactor.I8();

            return new LongVariant(res);
        }

        public override NumeralVariant Or(NumeralVariant or)
        {
            var res = _value | or.I8();

            return new LongVariant(res);
        }

        public override NumeralVariant Not()
        {
            return new LongVariant(~_value);
        }

        public override NumeralVariant And(NumeralVariant and)
        {
            return new LongVariant(_value & and.I8());
        }
        
        public override BaseVariant Hash()
        {
            // Perform bit-shifting and arithmetic operations for mutation
            long mutatedNumber = ((_value << Constants.NSalt1) + Constants.NSalt2) ^ Constants.NSalt3;

            // Ensure the mutated number is non-negative
            long absoluteMutatedNumber = Math.Abs(mutatedNumber);

            // Perform the hashing operation
            long hashedValue = ((absoluteMutatedNumber * Constants.NKey) % 1000) + Constants.NSalt3; // Modulus to keep the result within a reasonable range
            
            return new LongVariant(hashedValue);
        }
    }
}