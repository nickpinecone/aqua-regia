using System;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace AquaRegia.World.CoralReef.Tiles;

public class BreathBubble : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Sea/BubbleProjectile";

    public PropertyModule Property { get; private set; }
    public Animation<float> Accelerate { get; private set; }

    private Vector2 _spawnPosition;
    private float _offsetX;
    private int _direction;

    public BreathBubble()
    {
        Property = new PropertyModule(this);
        Accelerate = new Animation<float>(40, Ease.In);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this, 12, 12, 0, -1, 0, 50, 0, true, false, false);
        Property.SetTimeLeft(this, 180);

        DrawOriginOffsetY = -4;
    }

    public override void OnSpawn(IEntitySource source)
    {
        var iters = 0;

        while (TileHelper.IsSolid(Projectile.Center) && iters < 100)
        {
            iters++;

            Projectile.position.Y -= 4f;
        }

        _spawnPosition = Projectile.Center;
        _direction = Main.rand.NextFromList(-1, 1);
        _offsetX = Main.rand.NextFloat(8f, 16f);
        Projectile.velocity = new Vector2(Main.rand.NextFloat(1f, 2f) * _direction, Main.rand.NextFloat(-2f, -1f));

        Accelerate.Start = (int)Projectile.velocity.X;
        Accelerate.End = (int)Projectile.velocity.X;
    }

    public override void OnKill(int timeLeft)
    {
        foreach (var particle in Particle.Circle(DustID.BubbleBurst_Blue, Projectile.Center, new Vector2(4, 4), 4, 1f))
        {
            particle.noGravity = true;
        }
    }

    public override void AI()
    {
        if (Projectile.getRect().Intersects(Main.LocalPlayer.getRect()))
        {
            Main.LocalPlayer.breath += 3;
        }

        float acc = Accelerate.Animate(Accelerate.Start, Accelerate.End) ?? Accelerate.End;
        Projectile.velocity.X = acc;

        if (_direction == 1 && Projectile.Center.X > _spawnPosition.X + _offsetX)
        {
            Accelerate.Start = Projectile.velocity.X;
            Accelerate.End = -Projectile.velocity.X;
            Accelerate.Reset();

            _direction = -_direction;
            Projectile.velocity.X = -Projectile.velocity.X;
        }
        else if (_direction == -1 && Projectile.Center.X < _spawnPosition.X - _offsetX)
        {
            Accelerate.Start = Projectile.velocity.X;
            Accelerate.End = -Projectile.velocity.X;
            Accelerate.Reset();

            _direction = -_direction;
            Projectile.velocity.X = -Projectile.velocity.X;
        }
    }
}
