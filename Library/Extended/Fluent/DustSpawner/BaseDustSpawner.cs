using Microsoft.Xna.Framework;

namespace AquaRegia.Library.Extended.Fluent.DustSpawner;

public abstract class BaseDustSpawner
{
    protected int _dustType;
    protected bool _isPerfect = false;

    protected Vector2 _size = Vector2.Zero;
    protected float _scale = 1f;
    protected Vector2 _position = Vector2.Zero;

    protected Color _color = default;
    protected int _alpha = 0;
    protected float _fadeIn = 0f;

    protected BaseDustSpawner(int dustType)
    {
        _dustType = dustType;
    }

    protected BaseDustSpawner Perfect(bool isPerfect)
    {
        _isPerfect = isPerfect;
        return this;
    }

    protected BaseDustSpawner Size(Vector2 size, float scale = 1f)
    {
        _size = size;
        _scale = scale;
        return this;
    }

    protected BaseDustSpawner Position(Vector2 position)
    {
        _position = position;
        return this;
    }

    protected BaseDustSpawner Color(Color color, int alpha, float fadeIn = 0f)
    {
        _color = color;
        _alpha = alpha;
        _fadeIn =  fadeIn;
        return this;
    }
}