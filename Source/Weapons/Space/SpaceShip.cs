using System;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Players;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Weapons.Space;

public class SpaceShip : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Space/SpaceShip";

    public PropertyModule Property { get; private set; }
    public HomeModule Home { get; private set; }
    public SpriteModule Sprite { get; private set; }

    private int _direction = 0;
    private SpaceLaser? _laser = null;
    private Vector2 _start = Vector2.Zero;
    private Vector2 _end = Vector2.Zero;
    private float _distance = 320f;
    private float _offsetY = 256f;
    private Vector2 _endVelocity = Vector2.Zero;
    private bool _reachStart = true;
    private bool _laserPhase = false;
    private bool _bail = false;

    public SpaceShip() : base()
    {
        Property = new PropertyModule();
        Home = new HomeModule();
        Sprite = new SpriteModule();

        Composite.AddModule(Property, Home, Sprite);
    }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.projFrames[Projectile.type] = 4;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 38, 32, 0, -1, 0, 0, 0, false, false, false);
        Property.SetTimeLeft(this, 2);
        Home.SetDefaults();
    }

    private Vector2 GetRandomScreenEdge(Vector2 bounds, int direction)
    {
        var x = 0f;
        var y = 0f;

        y = Main.screenPosition.Y;

        if (direction > 0)
        {
            x = Main.rand.NextFloat(bounds.X - 720f, bounds.X - 280f);
        }
        else
        {
            x = Main.rand.NextFloat(bounds.X + 280f, bounds.X + 720f);
        }

        return new Vector2(x, y);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _direction = Math.Sign((Projectile.Center - Main.LocalPlayer.Center).X);
        _start = Projectile.Center - new Vector2(_distance / 2f * _direction, _offsetY);

        if (_start.Y < Main.screenPosition.Y)
        {
            Projectile.Center = _start - Vector2.UnitX * _direction * Main.rand.NextFloat(260f, 940f);
        }
        else
        {
            Projectile.Center = GetRandomScreenEdge(_start, _direction);
            Projectile.Center -= Projectile.Size;
            Projectile.velocity = Vector2.UnitY * 8f;
        }
    }

    public override void AI()
    {
        base.AI();

        Sprite.Animate(this, 6);
        Projectile.timeLeft = 2;

        if (_reachStart)
        {
            Projectile.velocity = Home.Calculate(Projectile.Center, Projectile.velocity, _start);

            if (Projectile.Center.DistanceSQ(_start) < 128f * 128f)
            {
                Home.Speed *= 0.9f;
            }

            if (Projectile.Center.DistanceSQ(_start) < 32f * 32f)
            {
                _reachStart = false;
                _laserPhase = true;
                Projectile.velocity = Vector2.Zero;
                Home.Speed = 16;
            }
        }
        else if (_laserPhase)
        {
            Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(2, 2);

            if (_laser == null)
            {
                _laser = Helper.SpawnProjectile<SpaceLaser>(Projectile.GetSource_FromThis(), Owner, Projectile.Center, Vector2.Zero, Projectile.damage, 0);
            }

            Projectile.Center += Vector2.UnitX * _direction * 2f;
            _laser.Projectile.Top = Projectile.Bottom + Vector2.UnitY * 12f;

            if (Projectile.Center.DistanceSQ(_start) > _distance * _distance)
            {
                _laserPhase = false;
                _laser.Projectile.Kill();

                _end = GetRandomScreenEdge(Projectile.Center, -_direction);
                _endVelocity = Vector2.UnitX * _direction * 8f;
            }
        }
        else
        {
            if (Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = _endVelocity;
            }

            if (!_bail)
            {
                Projectile.velocity = Home.Calculate(Projectile.Center, Projectile.velocity, _end);
            }
            else
            {
                Projectile.velocity *= 1.1f;
            }

            if (Projectile.Center.Y < Main.screenPosition.Y)
            {
                Projectile.Kill();
            }

            if (!_bail && Projectile.DistanceSQ(_end) < 32f * 32f)
            {
                _bail = true;
            }
        }
    }
}
