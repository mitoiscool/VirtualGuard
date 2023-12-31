namespace VirtualGuard.RT.Descriptor;

internal class ComparisonDescriptor
{
    public ComparisonDescriptor(Random rnd)
    {
        GtFlag = (byte)rnd.Next(byte.MaxValue);
        LtFlag = (byte)rnd.Next(byte.MaxValue);
        EqFlag = (byte)rnd.Next(byte.MaxValue);
    }
    
    
    public byte GtFlag;
    public byte LtFlag;
    public byte EqFlag;
}