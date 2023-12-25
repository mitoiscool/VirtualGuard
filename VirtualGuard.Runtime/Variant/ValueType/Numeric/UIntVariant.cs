using VirtualGuard.Runtime.Dynamic;

namespace VirtualGuard.Runtime.Variant.ValueType.Numeric;

public class UIntVariant : NumeralVariant
{
    private uint _value;

    public UIntVariant(uint i)
    {
        _value = i;
    }

    public override object GetObject()
    {
        return _value;
    }

    public override void SetVariantValue(object obj)
    {
        _value = (uint)obj;
    }

    public override BaseVariant Clone()
    {
        return new UIntVariant(_value);
    }

    public override NumeralVariant Add(NumeralVariant addend)
    {
        // result should always be int operating under realm of int
        var sum = _value + addend.U4();

        // do something with flags????

        return new UIntVariant(sum);
    }

    public override NumeralVariant Sub(NumeralVariant subtraend)
    {
        // result should always be int operating under realm of int
        var diff = _value - subtraend.U4();

        // do something with flags????

        return new UIntVariant(diff);
    }

    public override NumeralVariant Mul(NumeralVariant factor)
    {
        // result should always be int operating under realm of int
        var sum = _value * factor.U4();

        // do something with flags????

        return new UIntVariant(sum);
    }

    public override NumeralVariant Div(NumeralVariant divisor)
    {
        // result should always be int operating under realm of int
        var sum = _value + divisor.U4();

        // do something with flags????

        return new UIntVariant(sum);
    }

    public override NumeralVariant Xor(NumeralVariant xorfactor)
    {
        // result should always be int operating under realm of int
        var sum = _value ^ xorfactor.U4();

        // do something with flags????

        return new UIntVariant(sum);
    }

    public override NumeralVariant Rem(NumeralVariant remfactor)
    {
        // result should always be int operating under realm of int
        var sum = _value % remfactor.U4();

        // do something with flags????

        return new UIntVariant(sum);
    }

    public override NumeralVariant Or(NumeralVariant or)
    {
        // result should always be int operating under realm of int
        var sum = this.U4() | or.U4();

        // do something with flags????

        return new UIntVariant(sum);
    }

    public override NumeralVariant Not()
    {
        return new UIntVariant(~_value);
    }

    public override NumeralVariant And(NumeralVariant and)
    {
        return new UIntVariant((uint)(_value & and.U4()));
    }
    
    public override BaseVariant Hash()
    {
        // Perform bit-shifting and arithmetic operations for mutation
        uint mutatedNumber = ((_value << Constants.NSalt1) + Constants.NSalt2) ^ Constants.NSalt3;

        // Perform the hashing operation
        uint hashedValue = ((mutatedNumber * Constants.NKey) % 1000) + Constants.NSalt3; // Modulus to keep the result within a reasonable range
        
        return new UIntVariant(hashedValue);
    }
}