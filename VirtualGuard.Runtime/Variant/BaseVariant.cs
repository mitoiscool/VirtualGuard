using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using VirtualGuard.Runtime.Flags;
using VirtualGuard.Runtime.Variant.Member.impl;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.Variant
{

    public abstract class BaseVariant
    {
        public static BaseVariant CastVariant(object obj)
        {
            // populate with casts

            var type = Type.GetTypeCode(obj.GetType());

            switch (type)
            { // simple types
                case TypeCode.Byte:
                    return new ByteVariant((byte)obj);
                case TypeCode.SByte:
                    return new SByteVariant((sbyte)obj);
                
                case TypeCode.Int16:
                    return new ShortVariant((short)obj);
                
                case TypeCode.Int32:
                    return new IntVariant((int)obj);
                
                case TypeCode.Int64:
                    return new LongVariant((long)obj);
                
                case TypeCode.String:
                    return new StringVariant((string)obj);
            }

            if (obj is Array arr)
                return new ArrayVariant(arr);
            
            return new ObjectVariant(obj);
        }

        public static byte Compare(BaseVariant firstELement, BaseVariant secondElement)
        {
            byte flags = 0;
            

            if (firstELement.IsNumeral() && secondElement.IsNumeral())
            {
                // gt lt eq comparisons
                
                // cast to safe int (long in this case, no support for unsigned yet)

                if (firstELement.I8() > secondElement.I8())
                    flags |= (byte)ComparisonFlags.GT; // i'm not super sure how to use flags, could be &=

                if (firstELement.I8() < secondElement.I8())
                    flags |= (byte)ComparisonFlags.LT;

                if (firstELement.I8() == secondElement.I8())
                    flags |= (byte)ComparisonFlags.EQ;

                return flags;
            }

            if (firstELement.GetObject().Equals(secondElement.GetObject()))
                return (byte)ComparisonFlags.EQ;

            return (byte)ComparisonFlags.NEQ;
        }

        public virtual bool IsReference()
        {
            return false;
        }

        public virtual bool IsNumeral()
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
}