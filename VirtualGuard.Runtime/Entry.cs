using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime
{

    public class Entry
    {
        public static object VMEntryBasic(int loc, object[] args)
        {
            return VMEntry(loc, VMReader.GetEntryKey(loc), args);
        }

        public static object VMEntry(int loc, byte entryKey, object[] args)
        {
            return new VMContext(entryKey).Dispatch(loc, args);
        }

        public static object VMEntryInst(int loc, object[] args, object explicitInst)
        {
            var ctx = new VMContext(VMReader.GetEntryKey(loc));
            
            ctx.Stack.Push(new ObjectVariant(explicitInst));

            return ctx.Dispatch(loc, args);
        }

    }
}