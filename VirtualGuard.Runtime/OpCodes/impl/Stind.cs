using System.Reflection;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Stind : IOpCode
{
    public void Execute(VMContext ctx)
    {
        /*
         * var v2 = Pop();
			var v1 = Pop();
			v2 = Convert(v2, type);
			if (v1.IsReference())
				v2 = Convert(v2, v1.Type());
			else {
				if (v1.Value() is Pointer)
					unsafe { v1 = new PointerReference(new IntPtr(Pointer.Unbox(v1.Value())), type); }
				else
					throw new ArgumentException();
			}
			v1.SetValue(v2.Value());
         */

        var type = ctx.ResolveType(ctx.Reader.ReadInt());

        var v2 = ctx.Stack.Pop();
        var v1 = ctx.Stack.Pop();

        v2 = v2.Cast(type);
        
        if(v1.IsReference())
	        v2 = v2.Cast(v1.GetObject().GetType());
        else
        {
	        if(v1.GetObject() is Pointer)
		        unsafe
		        {
			        v1 = new PointerReferenceVariant(new IntPtr(Pointer.Unbox(v1.GetObject())), type);
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