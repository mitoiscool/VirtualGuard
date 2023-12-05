using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime;

public class VMStack {
    BaseVariant[] _array;
    uint _index;

    internal VMStack() {
        _array = new BaseVariant[10];
        _index = 0;
    }

    ~VMStack() {
        Array.Clear(_array, 0, _array.Length);
        _array = null;
        _index = 0;
    }

    internal void Push(BaseVariant val) {
        if (_index == _array.Length) {
            var arr = new BaseVariant[2 * _array.Length];
            Array.Copy(_array, 0, arr, 0, _index);
            _array = arr;
        }

        _array[_index++] = val;
    }

    internal BaseVariant Pop()
    {
        if (_index == 0)
            return null;

        var res = _array[--_index];
        _array[_index] = null;
        return res;
    }
}