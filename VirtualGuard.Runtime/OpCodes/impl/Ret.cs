using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Ret : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            // we can make this into a pseudo-code by setting state var
            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}