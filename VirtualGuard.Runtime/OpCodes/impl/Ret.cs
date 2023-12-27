using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Ret : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            // lol probably a better way to do this
        }

        public byte GetCode() => 0;
    }
}