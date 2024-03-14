namespace VirtualGuard.Runtime.OpCodes.impl.Stack
{

    public class Pop : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            ctx.Stack.Pop();
            
            
            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}