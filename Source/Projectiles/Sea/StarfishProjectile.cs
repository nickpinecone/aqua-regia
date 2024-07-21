using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Players;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Sea;

public class StarfishProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "StarfishProjectile";

    public PropertyModule Property { get; private set; }
    public AnimationModule Animation { get; private set; }
    public StickModule Stick { get; private set; }
    public BoomerangModule Boomerang { get; private set; }

    private Timer _stickTime;

    public StarfishProjectile() : base()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
        Stick = new StickModule(this);
        Boomerang = new BoomerangModule(this);

        _stickTime = new Timer(10, this, false);
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

        Boomerang.MaxPosition = 512f;
        Boomerang.Home.SetDefaults();
        Boomerang.Home.CurveChange = 1.01f;
        Boomerang.Home.Curve = 0.2f;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Boomerang.SpawnPosition = Projectile.Center;
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        SoundEngine.PlaySound(SoundID.NPCHit9);

        if (Stick.Target == null)
        {
            Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(4, 2);

            Stick.ToTarget(target, Projectile.Center);

            Stick.BeforeVelocity = Projectile.velocity;
            Projectile.velocity.Normalize();
            _stickTime.Restart();
        }

        var invert = Projectile.velocity.RotatedBy(MathHelper.Pi);
        var start = invert.RotatedBy(-MathHelper.PiOver4);
        var end = invert.RotatedBy(MathHelper.PiOver4);
        Particle.Arc(DustID.Blood, Stick.HitPoint, new Vector2(8, 8), start, end, 6, 2f, 1.2f);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        var particle = Particle.Single(DustID.Coralstone, Projectile.Center, new Vector2(8, 8), Vector2.Zero, 1f);
        particle.noGravity = true;
    }

    public override void AI()
    {
        base.AI();

        var isFar = Boomerang.Update(Projectile.Center);

        if (isFar && Stick.Target == null)
        {
            Projectile.velocity = Boomerang.ReturnToPlayer(Main.LocalPlayer, Projectile.Center, Projectile.velocity);

            if(Boomerang.Returned)
            {
                Projectile.Kill();
            }
        }

        if (Math.Abs(Projectile.velocity.X) > 0)
        {
            Projectile.rotation += 0.2f * Math.Sign(Projectile.velocity.X);
        }

        if (_stickTime.Done && Stick.Target != null)
        {
            Stick.Detach();
            Projectile.velocity = Stick.BeforeVelocity;
        }
    }
}
