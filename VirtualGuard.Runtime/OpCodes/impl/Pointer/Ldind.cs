using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl.Pointer;

public class Ldind : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var type = ctx.ResolveType(ctx.Reader.ReadInt());
        var v = ctx.Stack.Pop();

        if (!v.IsReference())
            unsafe
            {
                v = new PointerReferenceVariant(new IntPtr(System.Reflection.Pointer.Unbox(v.GetObject())), type);
            }
        
        ctx.Stack.Push(v);
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}