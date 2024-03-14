using VirtualGuard.Runtime.Dynamic;

namespace VirtualGuard.Runtime.OpCodes.impl.Exception;

public class Throw : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var exc = ctx.Stack.Pop().GetObject();

        if (exc is System.Exception ex)
            throw ex;

        throw new InvalidOperationException(Routines.EncryptDebugMessage("Popped exception could not be thrown."));
    }

    public byte GetCode() => 0;
}