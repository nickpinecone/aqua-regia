using AquaRegia.Library.Extended.Helpers;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Fluent.Spawners;

public class SingleDustSpawner : DustSpawner<SingleDustSpawner>
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

    public SingleDustSpawner Velocity(Vector2 velocity, bool noGravity = false)
    {
        _velocity = velocity;
        _noGravity = noGravity;
        return this;
    }
}