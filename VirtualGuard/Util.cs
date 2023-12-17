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
    
    public static void ReplaceRange<T>(this List<T> list, T existingItem, params T[] newItems)
    { // chatgpt ftw
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (existingItem == null)
        {
            throw new ArgumentNullException(nameof(existingItem));
        }

        int index = list.IndexOf(existingItem);

        if (index == -1)
        {
            throw new ArgumentException("The specified item does not exist in the list.", nameof(existingItem));
        }

        // Remove the existing item
        list.RemoveAt(index);

        // Insert new items at the removed item's index
        list.InsertRange(index, newItems);
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

    public static IMemberDefinition LookupMember(this ModuleDefinition mod, string name)
    {
        string[] split = name.Split(":");
        var targetType = mod.LookupType(split[0]);

        var members = new List<IMemberDefinition>();
        
        members.AddRange(targetType.Methods);
        members.AddRange(targetType.Fields);
        
        return members.Single(x => x.Name == split[1]);
    }
    
}