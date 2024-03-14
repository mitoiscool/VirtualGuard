using System.Reflection;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime.OpCodes.impl.Misc;

public class Ldtoken : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var member = ctx.ResolveMember(ctx.Reader.ReadInt());

        if (member is TypeInfo t)
        {
            ctx.Stack.Push(new ObjectVariant(t.TypeHandle));
        }

        if (member is MethodInfo m)
        {
            ctx.Stack.Push(new ObjectVariant(m.MethodHandle));
        }

        if (member is FieldInfo f)
        {
            ctx.Stack.Push(new ObjectVariant(f.FieldHandle));
        }

        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}