namespace VirtualGuard.RT.Descriptor;

public class HashDescriptor
{
    public HashDescriptor(Random rnd)
    {
        NKey = (byte)rnd.Next(255);
        
        NSalt1 = (byte)rnd.Next(255);
        NSalt2 = (byte)rnd.Next(255);
        NSalt3 = (byte)rnd.Next(255);

        SPolynomial = (uint)rnd.Next();
        SSeed = (uint)rnd.Next();
        SXorMask = (uint)rnd.Next();
    
        
    }

    public byte NKey;
    
    public byte NSalt1;
    public byte NSalt2;
    public byte NSalt3;

    public uint SPolynomial;
    public uint SSeed;
    public uint SXorMask;
}