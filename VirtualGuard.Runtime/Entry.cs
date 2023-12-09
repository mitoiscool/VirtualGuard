namespace VirtualGuard.Runtime
{

    public class Entry
    {
        public static object VMEntry(int loc, object[] args)
        {
            return new VMContext().Dispatch(loc, args);
        }


    }
}