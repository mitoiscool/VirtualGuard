using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes
{

    public interface IOpCode
    {
        void Execute(VMContext ctx);
        byte GetCode();
    }
}