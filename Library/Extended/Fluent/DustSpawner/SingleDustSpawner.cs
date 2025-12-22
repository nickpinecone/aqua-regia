using System;
using AquaRegia.Library.Extended.Helpers;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Fluent.DustSpawner;

public class SingleDustSpawner : BaseDustSpawner
{
    private Vector2 _velocity = Vector2.Zero;
    private bool _noGravity = false;

    public SingleDustSpawner(int dustType) : base(dustType)
    {
    }

    public Dust Spawn()
    {
        var dust = _isPerfect
            ? DustHelper.SinglePerfect(_dustType, _position, _velocity, _scale, _alpha, _color)
            : DustHelper.Single(_dustType, _position, _size, _velocity, _scale, _alpha, _color);

        dust.noGravity = _noGravity;
        dust.fadeIn = _fadeIn;

        return dust;
    }

    public new SingleDustSpawner Perfect(bool isPerfect)
    {
        base.Perfect(isPerfect);
        return this;
    }

    public new SingleDustSpawner Size(Vector2 size, float scale = 1f)
    {
        base.Size(size, scale);
        return this;
    }

    public new SingleDustSpawner Position(Vector2 position)
    {
        base.Position(position);
        return this;
    }

    public new SingleDustSpawner Color(Color color, int alpha, float fadeIn = 0f)
    {
        base.Color(color, alpha, fadeIn);
        return this;
    }

    public SingleDustSpawner Velocity(Vector2 velocity, bool noGravity = false)
    {
        _velocity = velocity;
        _noGravity = noGravity;
        return this;
    }
}