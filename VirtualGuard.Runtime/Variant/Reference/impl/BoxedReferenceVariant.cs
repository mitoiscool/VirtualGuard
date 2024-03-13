namespace VirtualGuard.Runtime.Variant.Reference.impl;

public class BoxedReferenceVariant : BaseReferenceVariant
{
    private BaseVariant _var;
    public BoxedReferenceVariant(BaseVariant var)
    {
        _var = var;
    }
    
    public override object GetObject()
    {
        return _var.GetObject();
    }

    public override void SetVariantValue(object obj)
    {
        _var.SetVariantValue(obj);
    }

    public override BaseVariant Clone()
    {
        return new BoxedReferenceVariant(_var);
    }

}