using System.Runtime.InteropServices;

namespace VirtualGuard.Runtime.Variant.Reference.impl;

public class PointerReferenceVariant : BaseReferenceVariant
{
    private IntPtr _ptr;
    private Type _t;
    
    public PointerReferenceVariant(IntPtr ptr, Type t)
    {
        _ptr = ptr;
        _t = t;
    }
    
    public override object GetObject()
    {
        return Marshal.PtrToStructure(_ptr, _t);
    }

    public override void SetVariantValue(object obj)
    {
        if (obj == null)
            throw new InvalidOperationException();
        
        Marshal.StructureToPtr(obj, _ptr, true);
    }

    public override BaseVariant Clone()
    {
        return new PointerReferenceVariant(_ptr, _t);
    }
}