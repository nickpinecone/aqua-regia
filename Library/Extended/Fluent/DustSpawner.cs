using AquaRegia.Library.Extended.Fluent.Spawners;
using Microsoft.Xna.Framework;

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

public abstract class DustSpawner<T>
    where T : DustSpawner<T>
{
    protected readonly int _dustType;
    protected bool _isPerfect = false;

    protected Vector2 _size = Vector2.Zero;
    protected float _scale = 1f;
    protected Vector2 _position = Vector2.Zero;

    protected Color _color = default;
    protected int _alpha = 0;
    protected float _fadeIn = 0f;

    protected DustSpawner(int dustType)
    {
        _dustType = dustType;
    }

    public T Perfect(bool isPerfect)
    {
        _isPerfect = isPerfect;
        return (T)this;
    }

    public T Size(Vector2 size, float scale = 1f)
    {
        _size = size;
        _scale = scale;
        return (T)this;
    }

    public T Position(Vector2 position)
    {
        _position = position;
        return (T)this;
    }

    public T Color(Color color, int alpha, float fadeIn = 0f)
    {
        _color = color;
        _alpha = alpha;
        _fadeIn = fadeIn;
        return (T)this;
    }
}