using System;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Projectiles.Explosion;

public class ExplosionProjectile : BaseProjectile
{
    public override string Texture => Assets.Empty;

    public PropertyModule Property { get; }
    public ImmunityModule Immunity { get; }
    public DataModule<ExplosionSource> Data { get; }

    public ExplosionProjectile()
    {
        Property = new PropertyModule(this);
        Immunity = new ImmunityModule();
        Data = new DataModule<ExplosionSource>();

        Composite.AddModule(Data, Property);
        Composite.AddRuntimeModule(Data, Immunity);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Size(16, 16)
            .Damage(DamageClass.Default, -1)
            .Friendly(true, false)
            .TimeLeft(15)
            .TileCollide(false);
    }

    public override bool? CanHitNPC(NPC target)
    {
        // TODO right now check only to Center, could miss even if it actually interlaps with the bounding rect
        // Need to check a circle with bounding rect collision instead
        return Projectile.Center.DistanceSQ(target.Center) < Math.Pow(Data.Source.Radius, 2) &&
               Immunity.CanHit(target) is null;
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.damage = Data.Source.Damage;
        Projectile.knockBack = Data.Source.KnockBack;
        Projectile.CritChance = Data.Source.CritChance;
        Projectile.DamageType = Data.Source.DamageType;
    }
}