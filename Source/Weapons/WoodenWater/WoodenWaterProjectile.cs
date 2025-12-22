using AquaRegia.Library;
using AquaRegia.Library.Extended.Extensions;
using AquaRegia.Library.Extended.Fluent;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Attributes;
using AquaRegia.Library.Extended.Modules.Projectiles;
using AquaRegia.Library.Extended.Modules.Sources;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.WoodenWater;

public class WoodenWaterProjectile : BaseProjectile
{
    public override string Texture => Assets.Sprites.Empty;

    private PropertyModule Property { get; } = new();

    [RuntimeModule] private DataModule<WeaponWithAmmoSource> Data { get; } = new();
    [RuntimeModule] private GravityModule Gravity { get; } = new();
    [RuntimeModule(1)] private WaterModule Water { get; } = new();
    [RuntimeModule] private ImmunityModule Immunity { get; } = new();

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Gravity.SetDefaults();

        Property.Set(this)
            .Size(16, 16)
            .Damage(DamageClass.Ranged, 1)
            .Friendly(true, false)
            .TimeLeft(35);
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Data.Source.Ammo?.ApplyToProjectile(this);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (Main.rand.Percent(25))
        {
            var position = target.Center - new Vector2(0, target.height * 1.5f + Main.rand.NextFloat(0f, 6f));

            new ProjectileSpawner<AcornProjectile>()
                .Context(Projectile.GetSource_FromThis(), Owner)
                .Position(position)
                .Damage(hit.Damage, hit.Knockback)
                .Spawn();
        }
    }
}