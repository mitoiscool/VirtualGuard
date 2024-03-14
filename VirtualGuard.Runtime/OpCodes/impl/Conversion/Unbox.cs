using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl.Conversion;

public class Unbox : IOpCode
{
    public void Execute(VMContext ctx)
    {
        unsafe
        {
            var type = ctx.ResolveType(ctx.Reader.ReadInt());

            var boxedObject = ctx.Stack.Pop();

            if (boxedObject is not BoxedReferenceVariant)
                throw new InvalidOperationException(
                    Routines.EncryptDebugMessage("Trying to unbox a non boxed variant."));

            var asPtr = new PointerReferenceVariant(
                new IntPtr(System.Reflection.Pointer.Unbox(boxedObject.GetObject())), type);
            
            ctx.Stack.Push(asPtr);
        }

        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}