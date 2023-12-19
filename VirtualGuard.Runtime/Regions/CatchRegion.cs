namespace VirtualGuard.Runtime.Regions;

public class CatchRegion : BaseRegion
{

    public CatchRegion(int pos, byte startKey) : base(pos, startKey)
    {
    }

    public override bool IsCatch()
    {
        return true;
    }
}
