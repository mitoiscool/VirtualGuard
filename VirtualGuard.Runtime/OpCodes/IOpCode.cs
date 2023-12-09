using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes
{

    public interface IOpCode
    {
        void Execute(VMContext ctx, out ExecutionState state);
        byte GetCode();
    }
}