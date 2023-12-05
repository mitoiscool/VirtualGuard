namespace VirtualGuard.Runtime.Variant.ValueType.Numeric;

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

    public override void SetFlags(int flag)
    {
        throw new NotImplementedException();
    }

    public override NumeralVariant Add(NumeralVariant addend)
    {
        // result should always be int operating under realm of int
        var sum = this.I4() + addend.I4();

        // do something with flags????

        return new IntVariant(sum);
    }

    public override NumeralVariant Sub(NumeralVariant subtraend)
    {
        // result should always be int operating under realm of int
        var diff = this.I4() - subtraend.I4();

        // do something with flags????

        return new IntVariant(diff);
    }

    public override NumeralVariant Mul(NumeralVariant factor)
    {
        // result should always be int operating under realm of int
        var sum = this.I4() * factor.I4();

        // do something with flags????

        return new IntVariant(sum);
    }

    public override NumeralVariant Div(NumeralVariant divisor)
    {
        // result should always be int operating under realm of int
        var sum = this.I4() + divisor.I4();

        // do something with flags????

        return new IntVariant(sum);
    }

    public override NumeralVariant Xor(NumeralVariant xorfactor)
    {
        // result should always be int operating under realm of int
        var sum = this.I4() ^ xorfactor.I4();

        // do something with flags????

        return new IntVariant(sum);
    }

    public override NumeralVariant Rem(NumeralVariant remfactor)
    {
        // result should always be int operating under realm of int
        var sum = this.I4() % remfactor.I4();

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

    public override NumeralVariant Not(NumeralVariant not)
    {
        throw new NotImplementedException();
    }
}