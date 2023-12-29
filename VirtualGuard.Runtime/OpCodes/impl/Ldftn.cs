using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Ldftn : IOpCode
{
    public void Execute(VMContext ctx)
    {
        ctx.Stack.Push(new ObjectVariant(ctx.ResolveMethod(ctx.Reader.ReadInt()).MethodHandle.GetFunctionPointer()));
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}