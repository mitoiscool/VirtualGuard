using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.Reference;
using VirtualGuard.Runtime.Variant.Reference.impl;
using VirtualGuard.Runtime.Variant.ValueType;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.Variant
{

    public abstract class BaseVariant
    {
        public static BaseVariant CreateVariant(object obj)
        {
            // populate with casts

            return CreateVariant(obj, obj.GetType());
        }

        public static BaseVariant CreateVariant(object obj, Type t)
        {
            switch (Type.GetTypeCode(t))
            { // corlib
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                    return new IntVariant(Convert.ToInt32(obj));
                
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                    return new UIntVariant((uint)obj);
                
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

        public BaseVariant Box()
        {
            if (this is BaseReferenceVariant)
                throw new InvalidOperationException(Routines.EncryptDebugMessage("Cannot box reference type."));
            
            
            return new BoxedReferenceVariant(this);
        }

        public BaseVariant Unbox()
        {
            if (this is BoxedReferenceVariant brv)
                return brv.Unbox();

            throw new InvalidOperationException(Routines.EncryptDebugMessage("Unboxing type could not be unboxed"));
        }
        
        public BaseVariant Cast(Type t)
        {
            
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

        public virtual BaseVariant GetLength()
        {
            throw new InvalidOperationException(Routines.EncryptDebugMessage("Could not get length of basevariant."));
        }
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
            return Convert.ToByte(GetObject());
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


        public virtual BaseVariant Hash()
        {
            throw new InvalidOperationException();
        }

    }
}