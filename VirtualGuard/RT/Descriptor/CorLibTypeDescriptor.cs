namespace VirtualGuard.RT.Descriptor;

internal class CorLibTypeDescriptor
{
    public CorLibTypeDescriptor(Random rnd)
    {
        I = rnd.Next();
        I1 = rnd.Next();
        I2 = rnd.Next();
        I4 = rnd.Next();
        I8 = rnd.Next();
        
        U = rnd.Next();
        U1 = rnd.Next();
        U2 = rnd.Next();
        U4 = rnd.Next();
        U8 = rnd.Next();
    }
    
    public int I;
    public int I1;
    public int I2;
    public int I4;
    public int I8;
    
    public int U;
    public int U1;
    public int U2;
    public int U4;
    public int U8;
}