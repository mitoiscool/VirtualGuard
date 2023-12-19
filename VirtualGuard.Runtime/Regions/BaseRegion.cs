namespace VirtualGuard.Runtime.Regions;

public abstract class BaseRegion
{
    public BaseRegion(int pos, byte startKey)
    {
        Position = pos;
        EntryKey = startKey;
    }

    public int Position;
    public byte EntryKey;

    public virtual bool IsFinally()
    {
        return false;
    }
    
    public virtual bool IsCatch()
    {
        return false;
    }
    
}