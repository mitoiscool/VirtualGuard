using AsmResolver.DotNet;

namespace VirtualGuard;

public static class Util
{
    public static void Shuffle<T>(this Random random, IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static TypeDefinition LookupType(this ModuleDefinition mod, string name)
    {
        return mod.GetAllTypes().Single(x => x.FullName == name);
    }
    
    public static MethodDefinition LookupMethod(this ModuleDefinition mod, string name)
    {
        string[] split = name.Split(":");
        
        return mod.GetAllTypes().Single(x => x.FullName == split[0]).Methods.Single(x => x.Name == split[1]);
    }
    
}