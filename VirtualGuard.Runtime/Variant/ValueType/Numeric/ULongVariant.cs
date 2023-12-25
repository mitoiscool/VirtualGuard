using VirtualGuard.Runtime.Dynamic;

namespace VirtualGuard.Runtime.Variant.ValueType.Numeric;

public class ULongVariant : NumeralVariant
{
    private ulong _value;

    public ULongVariant(ulong i)
    {
        _value = i;
    }

    public override object GetObject()
    {
        return _value;
    }

    public override void SetVariantValue(object obj)
    {
        _value = (ulong)obj;
    }

    public override BaseVariant Clone()
    {
        return new ULongVariant(_value);
    }

    public override NumeralVariant Add(NumeralVariant addend)
    {
        // result should always be int operating under realm of int
        var sum = _value + addend.U8();

        // do something with flags????

        return new ULongVariant(sum);
    }

    public override NumeralVariant Sub(NumeralVariant subtraend)
    {
        // result should always be int operating under realm of int
        var diff = _value - subtraend.U8();

        // do something with flags????

        return new ULongVariant(diff);
    }

    public override NumeralVariant Mul(NumeralVariant factor)
    {
        // result should always be int operating under realm of int
        var sum = _value * factor.U8();

        // do something with flags????

        return new ULongVariant(sum);
    }

    public override NumeralVariant Div(NumeralVariant divisor)
    {
        // result should always be int operating under realm of int
        var sum = _value + divisor.U8();

        // do something with flags????

        return new ULongVariant(sum);
    }

    public override NumeralVariant Xor(NumeralVariant xorfactor)
    {
        // result should always be int operating under realm of int
        var sum = _value ^ xorfactor.U8();

        // do something with flags????

        return new ULongVariant(sum);
    }

    public override NumeralVariant Rem(NumeralVariant remfactor)
    {
        // result should always be int operating under realm of int
        var sum = _value % remfactor.U8();

        // do something with flags????

        return new ULongVariant(sum);
    }

    public override NumeralVariant Or(NumeralVariant or)
    {
        // result should always be int operating under realm of int
        var sum = this.U8() | or.U8();

        // do something with flags????

        return new ULongVariant(sum);
    }

    public override NumeralVariant Not()
    {
        return new ULongVariant(~_value);
    }

    public override NumeralVariant And(NumeralVariant and)
    {
        return new ULongVariant((ulong)(_value & and.U8()));
    }
    
    public override BaseVariant Hash()
    { // should make these unique to make it extra-annoying
        // Perform bit-shifting and arithmetic operations for mutation
        ulong mutatedNumber = ((_value << Constants.NSalt1) + Constants.NSalt2) ^ Constants.NSalt3;

        // Perform the hashing operation
        ulong hashedValue = ((mutatedNumber * Constants.NKey) % 1000) + Constants.NSalt3; // Modulus to keep the result within a reasonable range
        
        return new ULongVariant(hashedValue);
    }
}