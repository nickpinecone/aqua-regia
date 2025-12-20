using AquaRegia.Library;
using AquaRegia.Library.Extended.Extensions;
using AquaRegia.Library.Extended.Helpers;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Projectiles;
using AquaRegia.Library.Extended.Modules.Sources;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.WoodenWater;

public class WoodenWaterProjectile : BaseProjectile
{
    public override string Texture => Assets.Empty;

    private DataModule<WeaponWithAmmoSource> Data { get; }
    private PropertyModule Property { get; }
    private WaterModule Water { get; }
    private GravityModule Gravity { get; }

    public WoodenWaterProjectile()
    {
        Data = new DataModule<WeaponWithAmmoSource>();
        Property = new PropertyModule(this);
        Water = new WaterModule();
        Gravity = new GravityModule();

        Composite.AddModule(Data, Property, Water, Gravity);
        Composite.AddRuntimeModule(Data, new ImmunityModule(), Water, Gravity);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Gravity.SetDefaults();

        Property.Size(16, 16)
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

            ModHelper.SpawnProjectile<AcornProjectile>(Projectile.GetSource_FromThis(), Owner, position, Vector2.Zero,
                hit.Damage, hit.Knockback);
        }
    }
}