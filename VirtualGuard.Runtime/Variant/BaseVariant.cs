using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.Variant
{

    public abstract class BaseVariant
    {
        public static BaseVariant CreateVariant(object obj)
        {
            // populate with casts

            var type = Type.GetTypeCode(obj.GetType());

            switch (type)
            { // simple types
                case TypeCode.Byte:
                    return new ByteVariant((byte)obj);
                
                case TypeCode.SByte:
                case TypeCode.Int16:
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
            if (firstELement.IsNumeral() && secondElement.IsNumeral())
            {
                // gt lt eq comparisons
                
                // cast to safe int (long in this case, no support for unsigned yet)

                if (firstELement.I8() > secondElement.I8())
                    return Constants.CMP_GT; // i'm not super sure how to use flags, could be &=

                if (firstELement.I8() < secondElement.I8())
                    return Constants.CMP_LT;

                if (firstELement.I8() == secondElement.I8())
                    return Constants.CMP_EQ;
                
            }

            if (firstELement.GetObject().Equals(secondElement.GetObject()))
                return Constants.CMP_EQ;

            return 0;
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
        public abstract void SetVariantValue(object obj);
        public abstract BaseVariant Clone();
        
        public virtual sbyte I1()
        {
            return Convert.ToSByte(GetObject());
        }

        public virtual short I2()
        {
            return Convert.ToInt16(GetObject());
        }

        public virtual int I4()
        {
            return Convert.ToInt32(GetObject());
        }

        public virtual long I8()
        {
            return Convert.ToInt64(GetObject());
        }

        public virtual byte U1()
        {
            return (byte)GetObject();
        }

        public virtual ushort U2()
        {
            return Convert.ToUInt16(GetObject());
        }

        public virtual uint U4()
        {
            return Convert.ToUInt32(GetObject());
        }

        public virtual ulong U8()
        {
            return Convert.ToUInt64(GetObject());
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
        

    }
}