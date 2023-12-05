namespace VirtualGuard.Runtime.Variant.ValueType.Numeric;

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

    public override void SetFlags(int flag)
    {
        throw new NotImplementedException();
    }

    public override NumeralVariant Add(NumeralVariant addend)
    {
        var sum = this.I8() + addend.I8();

        return new LongVariant(sum);
    }

    public override NumeralVariant Sub(NumeralVariant subtraend)
    {
        var diff = this.I8() - subtraend.I8();

        return new LongVariant(diff);
    }

    public override NumeralVariant Mul(NumeralVariant factor)
    {
        var product = this.I8() * factor.I8();

        return new LongVariant(product);
    }

    public override NumeralVariant Div(NumeralVariant divisor)
    {
        var quotient = this.I8() / divisor.I8();

        return new LongVariant(quotient);
    }

    public override NumeralVariant Xor(NumeralVariant xorfactor)
    {
        var result = this.I8() ^ xorfactor.I8();

        return new LongVariant(result);
    }

    public override NumeralVariant Rem(NumeralVariant remfactor)
    {
        var res = this.I8() % remfactor.I8();

        return new LongVariant(res);
    }

    public override NumeralVariant Or(NumeralVariant or)
    {
        var res = this.I8() | or.I8();

        return new LongVariant(res);
    }

    public override NumeralVariant Not(NumeralVariant not)
    {
        throw new NotImplementedException();
    }
}