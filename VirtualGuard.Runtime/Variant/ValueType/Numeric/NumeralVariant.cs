namespace VirtualGuard.Runtime.Variant.ValueType.Numeric;

public abstract class NumeralVariant : BaseVariant
{
    public abstract void SetFlags(int flag);
    
    public abstract NumeralVariant Add(NumeralVariant addend);
    public abstract NumeralVariant Sub(NumeralVariant subtraend);
    public abstract NumeralVariant Mul(NumeralVariant factor);
    public abstract NumeralVariant Div(NumeralVariant divisor);

    public abstract NumeralVariant Xor(NumeralVariant xorfactor);
    public abstract NumeralVariant Rem(NumeralVariant remfactor);
    public abstract NumeralVariant Or(NumeralVariant or);
    public abstract NumeralVariant Not(NumeralVariant not);
}