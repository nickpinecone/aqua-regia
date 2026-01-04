using System.Collections.Generic;
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
    private bool _noGravity = false;

    public ArcDustSpawner(int dustType) : base(dustType)
    {
    }

    public List<Dust> Spawn()
    {
        List<Dust> dusts;

        if (_isCircle)
        {
            dusts = _isPerfect
                ? DustHelper.CirclePerfect(_dustType, _position, _amount, _speed, _scale, _offset, _alpha, _color)
                : DustHelper.Circle(_dustType, _position, _size, _amount, _speed, _scale, _offset, _alpha, _color);
        }
        else
        {
            dusts = _isPerfect
                ? DustHelper.ArcPerfect(_dustType, _position, _startVector, _endVector, _amount, _speed, _scale,
                    _offset, _alpha, _color)
                : DustHelper.Arc(_dustType, _position, _size, _startVector, _endVector, _amount, _speed, _scale,
                    _offset, _alpha, _color);
        }

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
        return this;
    }
}