using AquaRegia.Library.Extended.Fluent.Spawners;

namespace AquaRegia.Library.Extended.Fluent;

public class DustSpawner
{
    private readonly int _dustType;

    public DustSpawner(int dustType)
    {
        _dustType = dustType;
    }

    public SingleDustSpawner Single()
    {
        return new SingleDustSpawner(_dustType);
    }
    
    public ArcDustSpawner Arc()
    {
        return new ArcDustSpawner(_dustType);
    }
}