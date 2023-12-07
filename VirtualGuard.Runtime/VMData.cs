namespace VirtualGuard.Runtime;

public class VMData
{
    public static VMData Load()
    {
        if (_inst != null)
            return _inst;

        _inst = new VMData();
        
        _inst.Extract();

        return new VMData();
    }

    private static VMData _inst;

    private Dictionary<uint, string> _stringMap = new Dictionary<uint, string>();

    public void Extract()
    {
        
        // get access to bytes somehow, I'll just use ms and a reader here
        
        
        
    }

    public string GetString(uint id)
    {
        return _stringMap[id];
    }

    public Stream GetVMData()
    {
        throw new NotImplementedException();
    }
    
    
    
}