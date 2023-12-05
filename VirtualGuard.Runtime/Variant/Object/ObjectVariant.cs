namespace VirtualGuard.Runtime.Variant.Object;

public class ObjectVariant : BaseVariant
{
    public ObjectVariant(object obj)
    {
        _obj = obj;
    }

    private object _obj;
    public override object GetObject()
    {
        return _obj;
    }
}