using VirtualGuard.Runtime.Dynamic;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Throw : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var exc = ctx.Stack.Pop().GetObject();

        if (exc is Exception ex)
            throw ex;

        throw new InvalidOperationException(Routines.EncryptDebugMessage("Popped exception could not be thrown."));
    }

    public byte GetCode() => 0;
}