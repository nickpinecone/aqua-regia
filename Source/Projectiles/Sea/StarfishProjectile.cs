using System;
using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Sea;

public class StarfishProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "StarfishProjectile";

    public PropertyModule Property { get; private set; }
    public AnimationModule Animation { get; private set; }
    public StickModule Stick { get; private set; }
    public BounceModule Bounce { get; private set; }
    public HomeModule Home { get; private set; }

    private Timer _spawnWait;
    private Vector2 _mouseWorld;
    private bool _reached;

    public StarfishProjectile() : base()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
        Stick = new StickModule(this);
        Bounce = new BounceModule(this, null);
        Home = new HomeModule(this);

        _spawnWait = new Timer(10, this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 600);

        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 20;
        Projectile.height = 18;

        Home.SetDefaults();
        Home.CurveChange = 1.01f;
        Home.Curve = 0.1f;

        // Infinite bounce
        Bounce.MaxCount = int.MaxValue;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _mouseWorld = Main.MouseWorld;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        base.OnTileCollide(oldVelocity);

        if (!_spawnWait.Done)
        {
            Projectile.velocity = Bounce.Update(null, oldVelocity, Projectile.velocity) ?? Projectile.velocity;

            return false;
        }
        else
        {
            return true;
        }
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        Stick.ToTarget(target, Projectile.Center);
    }

    public override void AI()
    {
        base.AI();

        if (_spawnWait.Done && Stick.Target == null && !_reached)
        {
            Projectile.velocity = Home.Calculate(Projectile.Center, Projectile.velocity, _mouseWorld);

            if (_mouseWorld.DistanceSQ(Projectile.Center) < 16f * 16f)
            {
                _reached = true;
            }
        }

        if (Stick.Target == null)
        {
            if (Math.Abs(Projectile.velocity.X) > 0)
            {
                Projectile.rotation += 0.1f * Math.Sign(Projectile.velocity.X);
            }
        }
        else
        {
            Projectile.Center = Stick.Update() ?? Projectile.Center;

            var upscale = Animation.Animate<float>("upscale", 1f, 1.2f, 20, Ease.InOut);
            Projectile.scale = upscale.Value ?? Projectile.scale;

            var downscale = Animation.Animate<float>("downscale", upscale.End, upscale.Start, upscale.Frames,
                                                     Ease.InOut, new string[] { "upscale" });
            Projectile.scale = downscale.Value ?? Projectile.scale;

            if (downscale.Finished)
            {
                upscale.Reset();
                downscale.Reset();

                upscale.End = Main.rand.NextFloat(1.1f, 1.3f);
            }
        }
    }
}
