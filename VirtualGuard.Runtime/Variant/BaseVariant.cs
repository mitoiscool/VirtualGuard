using VirtualGuard.Runtime.Variant.Member.impl;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.Variant;

public abstract class BaseVariant
{
    public static BaseVariant CastVariant(object obj)
    {
        // populate with casts



        return new ObjectVariant(obj);
    }
    
    
    public virtual bool IsReference()
    {
        return false;
    }

    public abstract object GetObject();

    public virtual sbyte I1()
    {
        return (sbyte)GetObject();
    }
    
    public virtual short I2()
    {
        return (short)GetObject();
    }
    
    public virtual int I4()
    {
        return (int)GetObject();
    }
    
    public virtual long I8()
    {
        return (long)GetObject();
    }
    
    public virtual byte U1()
    {
        return (byte)GetObject();
    }
    
    public virtual ushort U2()
    {
        return (ushort)GetObject();
    }
    
    public virtual uint U4()
    {
        return (uint)GetObject();
    }
    
    public virtual ulong U8()
    {
        return (ulong)GetObject();
    }

    public virtual string STR()
    {
        return GetObject().ToString();
    }

    public virtual NumeralVariant ToNumeral()
    {
        return (NumeralVariant)this;
    }
    
    public virtual ArrayVariant ToArray()
    {
        return (ArrayVariant)this;
    }

    public virtual FieldVariant ToField()
    {
        return (FieldVariant)this;
    }

}