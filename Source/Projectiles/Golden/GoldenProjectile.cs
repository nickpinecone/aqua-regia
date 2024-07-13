using System;
using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Players;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Golden;

public class GoldenProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public GoldenProjectile() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetDefaults(this);
        Property.SetDefaultGravity();
        Property.SetTimeLeft(this, 40);

        Projectile.damage = 1;
        Projectile.penetrate = 1;

        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Water.KillEffect(Projectile.Center, Projectile.velocity);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (Main.rand.Next(0, 3) == 0)
        {
            var offset = MathF.Max(target.width, target.height) * 2f;
            var offsetVector = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(offset, offset + 12f);
            var position = target.Center + offsetVector;

            var dagger = SpawnProjectile<DaggerProjectile>(position, Vector2.Zero, Projectile.damage / 2,
                                                           Projectile.knockBack / 2);
            offsetVector.Normalize();
            offsetVector = offsetVector.RotatedBy(MathHelper.Pi);
            dagger.InitialVelocity = offsetVector * 10f;
            dagger.Projectile.rotation = offsetVector.ToRotation() + MathHelper.PiOver2;
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Water.CreateDust(Projectile.Center, Projectile.velocity);

        if(Main.LocalPlayer.GetModPlayer<GoldenPlayer>().SwordCollide(Projectile.getRect()))
        {
            Projectile.Kill();
        }
    }

}
