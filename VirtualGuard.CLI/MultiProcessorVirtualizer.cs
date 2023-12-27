using AsmResolver.DotNet;

namespace VirtualGuard.CLI;

public class MultiProcessorVirtualizer
{
    private MultiProcessorAllocationMode _mode;
    
    private readonly Virtualizer[] _virtualizers;
    
    public MultiProcessorVirtualizer(VirtualGuardContext ctx, bool debug, int debugKey, MultiProcessorAllocationMode mode, int processors)
    {
        _mode = mode;

        _virtualizers = new Virtualizer[processors];

        for (int i = 0; i < processors; i++)
            _virtualizers[i] = new Virtualizer(ctx, debugKey, debug);
    }

    private List<MethodDefinition> _virtualizedMethods = new List<MethodDefinition>();
    
    private static Random _rnd = new Random();
    private int _index = 0;

    public void AddMethod(MethodDefinition def, bool export)
    {
        if (_virtualizedMethods.Contains(def))
            throw new InvalidOperationException("Method already virtualized.");
        
        _virtualizedMethods.Add(def);
        
        switch (_mode)
        {
            case MultiProcessorAllocationMode.Random:
                _virtualizers[_rnd.Next(_virtualizers.Length)].AddMethod(def, export);
                break;
            
            case MultiProcessorAllocationMode.Sequential:
                if (_index == _virtualizers.Length)
                    _index = 0; // reset to 0, at capacity
                else
                    _index++;

                _virtualizers[_index].AddMethod(def, export);
                break;
            
        }
    }

    public void Commit()
    {
        foreach (var virt in _virtualizers)
            virt.CommitRuntime();
    }
    
}

public enum MultiProcessorAllocationMode
{
    Random,
    Sequential
}