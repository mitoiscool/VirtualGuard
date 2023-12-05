namespace VirtualGuard.Runtime;

public class Entry
{
    public static object CallVM(int loc, object[] args)
    {
        return new VMContext(VMData.Load()).Dispatch(args);
    }
}