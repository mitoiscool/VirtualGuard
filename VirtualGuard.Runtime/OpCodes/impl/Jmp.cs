using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Jmp : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var key = ctx.Stack.Pop().U1();
            
            ctx.Reader.SetKey(key);
            ctx.Reader.SetValue(ctx.Stack.Pop().I4());
            
            ctx.CurrentCode = ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}