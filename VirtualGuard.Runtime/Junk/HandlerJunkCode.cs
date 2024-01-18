using VirtualGuard.Runtime.OpCodes;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.Junk;

public class HandlerJunkCode
{
    public void OpaqueBranchReadInt(VMContext ctx)
    {
        ctx.Stack.Push(new NullVariant());
        if (ctx.Stack.Pop().IsNumeral())
        {
            ctx.Stack.Push(new IntVariant(ctx.Reader.ReadInt()));
        }
        
    }
    
    public void OpaqueBranchReadLong(VMContext ctx)
    {
        ctx.Stack.Push(new NullVariant());
        if (ctx.Stack.Pop().IsReference())
        {
            ctx.Stack.Push(new LongVariant(ctx.Reader.ReadLong()));
        }
        
    }
    
    public void OpaqueBranchBranch(VMContext ctx)
    {
        ctx.Stack.Push(new NullVariant());
        if (ctx.Stack.Peek().IsNumeral())
        {
            ctx.Reader.
        }

        ctx.Stack.Pop();

    }
    
}