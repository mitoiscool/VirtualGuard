using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime.OpCodes.impl.Array;

public class Newarr : IOpCode
{
    public void Execute(VMContext ctx)
    {
        ctx.Stack.Push(new ArrayVariant(System.Array.CreateInstance(ctx.ResolveType(ctx.Reader.ReadInt()), ctx.Stack.Pop().I4())));

        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}