using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime
{

    public class Entry
    {
        public static object VMEntry(int loc, object[] args)
        {
            return VMEntry(loc, VMReader.GetEntryKey(loc), args);
        }

        public static object VMEntry(int loc, byte entryKey, object[] args)
        {
            return new VMContext(entryKey).Dispatch(loc, args);
        }

    }
}