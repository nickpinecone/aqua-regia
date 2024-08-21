using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AquaRegia.Weapons.Sunflower;

public class BloodVine : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public ChainModule Chain { get; private set; }
    public StickModule Stick { get; private set; }

    public Timer HealTimer { get; private set; }

    public BloodVine() : base()
    {
        Property = new PropertyModule(this);
        Chain = new ChainModule(this);
        Stick = new StickModule(this);

        HealTimer = new Timer(60, this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 2);
        Chain.SetTexture(TexturesPath.Weapons + "Sunflower/BloodVine", new Rectangle(0, 0, 24, 28));

        Projectile.damage = 1;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;

        Projectile.width = 24;
        Projectile.height = 24;

        Projectile.hostile = false;
        Projectile.friendly = true;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Main.LocalPlayer.GetModPlayer<SunflowerPlayer>().BloodVine = this;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Main.LocalPlayer.GetModPlayer<SunflowerPlayer>().BloodVine = null;

        var direction = Projectile.Center - Main.LocalPlayer.Top;
        var unit = direction.SafeNormalize(Vector2.Zero);
        for (int i = 0; i < (direction.Length() / 26); i++)
        {
            Particle.Single(DustID.CrimsonPlants, Projectile.Center - unit * i * 26, new Vector2(4, 4), Vector2.Zero,
                            1f);
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (HealTimer.Done)
        {
            Main.LocalPlayer.Heal(1);
            HealTimer.Restart();
        }

        if (Stick.Target == null)
        {
            Projectile.velocity = Vector2.Zero;
            Stick.ToTarget(target, Projectile.Center);
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.timeLeft = 2;

        if (Main.LocalPlayer.Center.DistanceSQ(Projectile.Center) > 1000f * 1000f)
        {
            Projectile.Kill();
        }

        if (Stick.Target == null)
        {
            var target = Helper.FindNearestNPC(Projectile.Center, 1000f);

            if (target == null)
            {
                Projectile.Kill();
            }
            else
            {
                var direction = (target.Center - Projectile.Center);
                direction.Normalize();
                direction *= 16f;

                Projectile.velocity = direction;
            }
        }
        else
        {
            var result = Stick.Update();

            if (result == null)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.Center = Stick.Target.Center;
            }
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        base.PreDraw(ref lightColor);

        var sunflower = Main.LocalPlayer.GetModPlayer<SunflowerPlayer>().Sunflower;

        if (sunflower == null || Main.IsItDay())
        {
            Projectile.Kill();
        }
        else
        {
            var direction = Projectile.Center - Main.LocalPlayer.Top;
            direction.Normalize();

            Chain.DrawChain(Projectile.Center, Main.LocalPlayer.Top + direction * 16f);
        }

        return true;
    }
}
