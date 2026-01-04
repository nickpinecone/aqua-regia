using System.Collections.Generic;
using System.Linq;
using AquaRegia.Library.Extended.Helpers;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Fluent.Spawners;

public class ArcDustSpawner : DustSpawner<ArcDustSpawner>
{
    private float _offset = 0f;
    private float _speed = 0f;
    private bool _isCircle = false;

    private Vector2 _startVector = Vector2.Zero;
    private Vector2 _endVector = Vector2.Zero;
    private int _amount = 0;

    public ArcDustSpawner(int dustType) : base(dustType)
    {
    }

    public IEnumerable<Dust> Spawn()
    {
        var dusts = DustHelper.GenerateArc(
            _isPerfect, !_isCircle, _dustType, _position, _size, _startVector,
            _endVector, _amount, _speed, _scale, _offset, _alpha, _color
        ).ToArray();

        foreach (var dust in dusts)
        {
            dust.fadeIn = _fadeIn;
            dust.noGravity = _noGravity;
        }

        return dusts;
    }

    public ArcDustSpawner Position(Vector2 position, float offset = 0f)
    {
        base.Position(position);
        _offset = offset;
        return this;
    }

    public ArcDustSpawner Speed(float speed, bool noGravity = false)
    {
        _speed = speed;
        _noGravity = noGravity;
        return this;
    }

    public ArcDustSpawner Edges(Vector2 startVector, Vector2 endVector, int amount)
    {
        _startVector = startVector;
        _endVector = endVector;
        _amount = amount;
        return this;
    }

    public ArcDustSpawner Circle(int amount)
    {
        _amount = amount;
        _isCircle = true;
        _startVector = Vector2.UnitX;
        _endVector = Vector2.UnitX;
        return this;
    }
}