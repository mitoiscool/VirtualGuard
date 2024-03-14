using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl.Pointer;

public class Stind : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var type = ctx.ResolveType(ctx.Reader.ReadInt());

        var v2 = ctx.Stack.Pop();
        var v1 = ctx.Stack.Pop();

        v2 = v2.Cast(type);
        
        if(v1.IsReference())
	        v2 = v2.Cast(v1.GetObject().GetType());
        else
        {
	        if(v1.GetObject() is System.Reflection.Pointer)
		        unsafe
		        {
			        v1 = new PointerReferenceVariant(new IntPtr(System.Reflection.Pointer.Unbox(v1.GetObject())), type);
		        }
	        else
	        {
		        throw new ArgumentException();
	        }
        }
        
        v1.SetVariantValue(v2.GetObject());
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}