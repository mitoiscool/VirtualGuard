namespace VirtualGuard.Runtime.Variant.Member;

public abstract class BaseMemberVariant : BaseVariant
{
    public abstract void SetValue(BaseVariant inst, BaseVariant value);
    public abstract object GetValue(BaseVariant inst);
}