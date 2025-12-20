using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Projectiles;
using AquaRegia.Library.Extended.Modules.Sources;
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
            .Damage(1, 0f, DamageClass.Ranged, 1)
            .TimeLeft(35);
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Data.Source.Ammo?.ApplyToProjectile(this);
    }
}