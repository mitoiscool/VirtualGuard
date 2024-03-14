namespace VirtualGuard.Runtime.OpCodes.impl.Field
{

    public class Stfld : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var field = ctx.ResolveField(ctx.Stack.Pop().I4());

            object inst = null;

            if (!field.IsStatic)
                inst = ctx.Stack.Pop();

            field.SetValue(inst, ctx.Stack.Pop().GetObject());

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}