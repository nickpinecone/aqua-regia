using System;
using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Projectiles.Modules;

namespace WaterGuns.Projectiles.Wooden;

public class AcornProjectile : BaseProjectile
{
    public override string Texture => "WaterGuns/Assets/Textures/Projectiles/AcornProjectile";

    public PropertyModule Property { get; private set; }
    public AnimationModule Animation { get; private set; }
    public HeadBounceModule HeadBounce { get; private set; }

    public AcornProjectile() : base()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
        HeadBounce = new HeadBounceModule(this, Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetDefaultGravity();
        Property.SetTimeLeft(this, 120);

        Projectile.damage = 1;
        Projectile.penetrate = 5;

        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.alpha = 255;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.rotation = Main.rand.NextFloat(0f, MathHelper.TwoPi);
    }

    public override bool? CanHitNPC(NPC target)
    {
        if(!HeadBounce.CanHit(target, Projectile.Center))
            return false;

        return base.CanHitNPC(target);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        var dustWood = 7;
        var dust = Dust.NewDustDirect(Projectile.Center, 16, 16, dustWood, 0, 0, 0, default, 1);
        dust.noGravity = true;

        Projectile.velocity = HeadBounce.BounceOff(target, Projectile.Center) ?? Projectile.velocity;
    }

    public override void AI()
    {
        base.AI();

        Projectile.alpha = (int)(Animation.AnimateF("appear", 255, 0, 10, new string[] {}) ?? Projectile.alpha);

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);

        if (Math.Abs(Projectile.velocity.X) > 0)
        {
            Projectile.rotation += 0.1f * Math.Sign(Projectile.velocity.X);
        }
    }
}
