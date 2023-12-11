namespace VirtualGuard.RT.Descriptor;

public class ComparisonDescriptor
{
    public ComparisonDescriptor()
    {
        var rnd = new Random();

        GtFlag = (byte)rnd.Next(byte.MaxValue);
        LtFlag = (byte)rnd.Next(byte.MaxValue);
        EqFlag = (byte)rnd.Next(byte.MaxValue);
    }
    
    
    public byte GtFlag;
    public byte LtFlag;
    public byte EqFlag;
}