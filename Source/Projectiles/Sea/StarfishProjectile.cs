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
    public HomeModule Home { get; private set; }

    private NPC _oldTarget;
    private Timer _stickTime;
    private Vector2 _beforeVelocity;
    private Timer _homeTime;

    public StarfishProjectile() : base()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
        Stick = new StickModule(this);
        Home = new HomeModule(this);

        _stickTime = new Timer(15, this);
        _stickTime.Paused = true;

        _homeTime = new Timer(120, this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        _immunity.ImmunityTime = 5;

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 600);

        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 20;
        Projectile.height = 18;

        Home.SetDefaults();
        Home.CurveChange = 1.01f;
        Home.Curve = 2f;
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        SoundEngine.PlaySound(SoundID.NPCHit9);

        if (Stick.Target == null)
        {
            Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(5, 1);

            Stick.ToTarget(target, Projectile.Center);

            _beforeVelocity = Projectile.velocity;
            Projectile.velocity.Normalize();
            _stickTime.Restart();
        }
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

        if (_homeTime.Done && Stick.Target == null)
        {
            var slow = Animation.Animate<Vector2>("slow", Projectile.velocity, Vector2.Zero, 20, Ease.InOut);

            if (slow.Finished)
            {
                Projectile.velocity = Home.Calculate(Projectile.Center, Projectile.velocity, Main.LocalPlayer.Center);

                if (Main.LocalPlayer.Center.DistanceSQ(Projectile.Center) < 16f * 16f)
                {
                    Projectile.Kill();
                }
            }
        }

        if (Math.Abs(Projectile.velocity.X) > 0)
        {
            Projectile.rotation += 0.2f * Math.Sign(Projectile.velocity.X);
        }

        if (_stickTime.Done && Stick.Target != null)
        {
            Stick.Detach();
            Projectile.velocity = _beforeVelocity;
        }
    }
}
