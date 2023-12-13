using System;

namespace VirtualGuard.Runtime.Variant.Object
{

    public class ArrayVariant : BaseVariant
    {
        private Array _array;

        public ArrayVariant(Array arr)
        {
            _array = arr;
        }

        public override object GetObject()
        {
            return _array;
        }

        public override void SetVariantValue(object obj)
        {
            _array = (Array)obj;
        }

        public override BaseVariant Clone()
        {
            return new ArrayVariant(_array);
        }

        public BaseVariant LoadDelimeter(BaseVariant index)
        {
            return BaseVariant.CreateVariant(_array.GetValue(index.I4()));
        }

        public void SetDelimeter(BaseVariant index, BaseVariant obj)
        {
            _array.SetValue(obj.GetObject(), index.I4());
        }

    }
}