namespace VirtualGuard.RT.Descriptor;

public class HashDescriptor
{
    public HashDescriptor(Random rnd)
    {
        NKey = (byte)rnd.Next(255);
        
        NSalt1 = (byte)rnd.Next(255);
        NSalt2 = (byte)rnd.Next(255);
        NSalt3 = (byte)rnd.Next(255);
        
        SSalt1 = (byte)rnd.Next(255);
        SSalt2 = (byte)rnd.Next(255);
        SSalt3 = (byte)rnd.Next(255);
        
    }

    public byte NKey;
    
    public byte NSalt1;
    public byte NSalt2;
    public byte NSalt3;

    public byte SSalt1;
    public byte SSalt2;
    public byte SSalt3;
}