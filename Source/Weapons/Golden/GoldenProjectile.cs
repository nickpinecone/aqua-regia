using System;
using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Weapons.Golden;

public class GoldenProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public GoldenProjectile() : base()
    {
        var immunity = new ImmunityModule();
        immunity.SetDefaults();
        Composite.AddRuntimeModule(immunity);

        Property = new PropertyModule();
        Water = new WaterModule();

        Composite.AddModule(Property, Water);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetProperties(this, 16, 16, 1, 1);
        Property.SetGravity();
        Property.SetTimeLeft(this, 40);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        if (source is WeaponWithAmmoSource custom)
        {
            custom.Ammo?.ApplyToProjectile(this);
        }
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

            var dagger =
                Helper.SpawnProjectile<DaggerProjectile>(Projectile.GetSource_FromThis(), Owner, position, Vector2.Zero,
                                                         Projectile.damage / 2, Projectile.knockBack / 2);
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

        if (Owner.GetModPlayer<GoldenPlayer>().SwordCollide(Projectile.getRect()))
        {
            Projectile.Kill();
        }
    }
}
