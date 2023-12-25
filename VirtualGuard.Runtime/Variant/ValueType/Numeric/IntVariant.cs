using System;
using VirtualGuard.Runtime.Dynamic;

namespace VirtualGuard.Runtime.Variant.ValueType.Numeric
{

    public class IntVariant : NumeralVariant
    {
        private int _value;

        public IntVariant(int i)
        {
            _value = i;
        }

        public override object GetObject()
        {
            return _value;
        }

        public override void SetVariantValue(object obj)
        {
            _value = (int)obj;
        }

        public override BaseVariant Clone()
        {
            return new IntVariant(_value);
        }

        public override NumeralVariant Add(NumeralVariant addend)
        {
            // result should always be int operating under realm of int
            var sum = _value + addend.I4();

            // do something with flags????

            return new IntVariant(sum);
        }

        public override NumeralVariant Sub(NumeralVariant subtraend)
        {
            // result should always be int operating under realm of int
            var diff = _value - subtraend.I4();

            // do something with flags????

            return new IntVariant(diff);
        }

        public override NumeralVariant Mul(NumeralVariant factor)
        {
            // result should always be int operating under realm of int
            var sum = _value * factor.I4();

            // do something with flags????

            return new IntVariant(sum);
        }

        public override NumeralVariant Div(NumeralVariant divisor)
        {
            // result should always be int operating under realm of int
            var sum = _value + divisor.I4();

            // do something with flags????

            return new IntVariant(sum);
        }

        public override NumeralVariant Xor(NumeralVariant xorfactor)
        {
            // result should always be int operating under realm of int
            var sum = _value ^ xorfactor.I4();

            // do something with flags????

            return new IntVariant(sum);
        }

        public override NumeralVariant Rem(NumeralVariant remfactor)
        {
            // result should always be int operating under realm of int
            var sum = _value % remfactor.I4();

            // do something with flags????

            return new IntVariant(sum);
        }

        public override NumeralVariant Or(NumeralVariant or)
        {
            // result should always be int operating under realm of int
            var sum = this.I4() | or.I4();

            // do something with flags????

            return new IntVariant(sum);
        }

        public override NumeralVariant Not()
        {
            return new IntVariant(~_value);
        }

        public override NumeralVariant And(NumeralVariant and)
        {
            return new IntVariant((int)(_value & and.I4()));
        }

        public override BaseVariant Hash()
        {
            // Perform bit-shifting and arithmetic operations for mutation
            int mutatedNumber = ((_value << Constants.NSalt1) + Constants.NSalt2) ^ Constants.NSalt3;

            // Ensure the mutated number is non-negative
            int absoluteMutatedNumber = Math.Abs(mutatedNumber);

            // Perform the hashing operation
            int hashedValue = ((absoluteMutatedNumber * Constants.NKey) % 1000) + Constants.NSalt3; // Modulus to keep the result within a reasonable range
            
            return new IntVariant(hashedValue);
        }
        
        
    }
}