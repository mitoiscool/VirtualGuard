using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Entertry : IOpCode
{
    public void Execute(VMContext ctx)
    {

        var loc = ctx.Stack.Pop();
        var type = ctx.Stack.Pop();
        var entry = ctx.Reader.ReadByte();
        
        ctx.HandlerStack.Push(new ExceptionHandler(type.U1(), entry.U1(), loc.I4()));

        ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}