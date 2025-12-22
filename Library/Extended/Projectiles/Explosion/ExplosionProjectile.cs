using System;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Attributes;
using AquaRegia.Library.Extended.Modules.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Projectiles.Explosion;

public class ExplosionProjectile : BaseProjectile
{
    public override string Texture => Assets.Empty;

    private PropertyModule Property { get; } = new();

    [RuntimeModule] private DataModule<ExplosionSource> Data { get; } = new();
    [RuntimeModule] private ImmunityModule Immunity { get; } = new();

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Set(this)
            .Size(16, 16)
            .Damage(DamageClass.Default, -1)
            .Friendly(true, false)
            .TimeLeft(15)
            .TileCollide(false);
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        return projHitbox.Center().DistanceSQ(targetHitbox.Center()) < Math.Pow(Data.Source.Radius, 2);
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.CritChance = Data.Source.CritChance;
        Projectile.DamageType = Data.Source.DamageType;
    }
}