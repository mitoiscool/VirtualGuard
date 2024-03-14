namespace VirtualGuard.Runtime.Variant.ValueType.Numeric
{

    public abstract class NumeralVariant : BaseVariant
    {
        public override bool IsNumeral()
        {
            return true;
        }

        public abstract NumeralVariant Add(NumeralVariant addend);
        public abstract NumeralVariant Sub(NumeralVariant subtraend);
        public abstract NumeralVariant Mul(NumeralVariant factor);
        public abstract NumeralVariant Div(NumeralVariant divisor);

        public abstract NumeralVariant Xor(NumeralVariant xorfactor);
        public abstract NumeralVariant Rem(NumeralVariant remfactor);
        public abstract NumeralVariant Or(NumeralVariant or);
        public abstract NumeralVariant Not();
        public abstract NumeralVariant And(NumeralVariant and);

        public abstract NumeralVariant Shl(NumeralVariant factor);
        public abstract NumeralVariant Shr(NumeralVariant factor);
    }
}