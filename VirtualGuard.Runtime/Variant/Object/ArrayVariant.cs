namespace VirtualGuard.Runtime.Variant.Object;

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

    public object LoadDelimeter(BaseVariant index)
    {
        return _array.GetValue(index.I4());
    }

    public void SetDelimeter(BaseVariant index, BaseVariant obj)
    {
        _array.SetValue(obj.GetObject(), index.I4());
    }
    
}