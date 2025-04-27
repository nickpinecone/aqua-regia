using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace WaterGuns.Weapons.Sunflower;

public class BloodVine : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public ChainModule Chain { get; private set; }
    public StickModule Stick { get; private set; }

    public Timer HealTimer { get; private set; }

    public BloodVine() : base()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();
        Chain = new ChainModule();
        Stick = new StickModule();

        Composite.AddModule(Property, Chain, Stick);

        HealTimer = new Timer(60);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 24, 24, 1, -1, 0f, 0, 0, false);
        Property.SetTimeLeft(this, 2);
        Chain.SetTexture(TexturesPath.Weapons + "Sunflower/BloodVine", new Rectangle(0, 0, 24, 28));
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Owner.GetModPlayer<SunflowerPlayer>().BloodVine = this;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Owner.GetModPlayer<SunflowerPlayer>().BloodVine = null;

        var direction = Projectile.Center - Owner.Top;
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
            Owner.Heal(1);
            HealTimer.Restart();
        }

        if (Stick.Target == null)
        {
            Projectile.velocity = Vector2.Zero;
            Stick.ToTarget(target, Projectile.Center);
        }
    }

    public override bool PreAI()
    {
        HealTimer.Update();

        return true;
    }

    public override void AI()
    {
        base.AI();

        Projectile.timeLeft = 2;

        if (Owner.DistanceSQ(Projectile.Center) > 1000f * 1000f)
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

        var sunflower = Owner.GetModPlayer<SunflowerPlayer>().Sunflower;

        if (sunflower == null || Main.IsItDay())
        {
            Projectile.Kill();
        }
        else
        {
            var direction = Projectile.Center - Owner.Top;
            direction.Normalize();

            Chain.DrawChain(Projectile.Center, Owner.Top + direction * 16f);
        }

        return true;
    }
}
