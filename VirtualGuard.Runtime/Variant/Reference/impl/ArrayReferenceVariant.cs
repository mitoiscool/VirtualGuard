using System;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.Variant.Reference.impl
{
    public class ArrayReferenceVariant : BaseReferenceVariant
    {
        private ArrayVariant _array;
        private BaseVariant _index;
        
        public ArrayReferenceVariant(ArrayVariant arr, BaseVariant index)
        {
            _array = arr;
            _index = index;

            if (index.GetType() != typeof(IntVariant))
                throw new ArgumentException();
            
        }

        public override object GetObject()
        {
            return _array.LoadDelimeter(_index);
        }

        public override void SetValue(object obj)
        {
            _array.SetDelimeter(_index, CreateVariant(obj));
        }

        public override BaseVariant Clone()
        {
            return new ArrayReferenceVariant(_array, _index);
        }
    }
}