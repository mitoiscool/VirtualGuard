using System;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime
{

    public class VMStack : IElement
    {
        BaseVariant[] _array;
        uint _index;

        internal VMStack()
        {
            _array = new BaseVariant[10];
            _index = 0;
        }

        internal void Push(BaseVariant val)
        {
            //Console.WriteLine("push: " + val.STR());
            
            if (_index == _array.Length)
            {
                var arr = new BaseVariant[2 * _array.Length];
                Array.Copy(_array, 0, arr, 0, _index);
                _array = arr;
            }
            
            _array[_index++] = val;
        }

        internal BaseVariant Pop()
        {
            if (_index == 0)
                return new NullVariant();
            
            var res = _array[--_index];
            _array[_index] = null;
            
            //Console.WriteLine("pop: " + res.STR());
            return res;
        }
        
        internal BaseVariant Peek()
        {
            //Console.WriteLine("peeked " + _array[_index - 1].STR());
            return _array[_index - 1];
        }

        public void SetValue(int i)
        {
            _index = (uint)i;
        }

        public int GetValue()
        {
            return (int)_index;
        }
    }
}