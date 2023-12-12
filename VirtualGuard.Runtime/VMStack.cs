using System;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime
{

    public class VMStack
    {
        BaseVariant[] _array;
        uint _index;

        internal VMStack()
        {
            _array = new BaseVariant[10];
            _index = 0;
        }

        ~VMStack()
        {
            Array.Clear(_array, 0, _array.Length);
            _array = null;
            _index = 0;
        }

        internal void Push(BaseVariant val)
        {
            Console.WriteLine("push: " + val.STR());
            
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
            var res = _array[--_index];
            _array[_index] = new NullVariant();
            
            Console.WriteLine("pop: " + res.STR());
            return res;
        }


        internal BaseVariant Peek()
        {
            return _array[_index];
        }

    }
}