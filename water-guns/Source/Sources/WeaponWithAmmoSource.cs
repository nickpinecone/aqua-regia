using Terraria.DataStructures;
using Terraria.ModLoader;
using WaterGuns.Modules.Ammo;
using WaterGuns.Modules.Weapons;

namespace WaterGuns.Sources;

public class WeaponWithAmmoSource : IEntitySource
{
    public BaseWeapon Weapon { get; private set; }
    public BaseAmmo? Ammo { get; private set; }

    public string? Context { get; set; }

    public WeaponWithAmmoSource(WeaponWithAmmoSource source)
        : this((IEntitySource)source, source.Weapon, source.Ammo)
    {
    }

    public WeaponWithAmmoSource(EntitySource_ItemUse_WithAmmo source, BaseWeapon weapon)
        : this((IEntitySource)source, weapon, (BaseAmmo)ModContent.GetModItem(source.AmmoItemIdUsed))
    {
    }

    public WeaponWithAmmoSource(IEntitySource source, BaseWeapon weapon, BaseAmmo? ammo = null)
    {
        Context = source.Context;
        Weapon = weapon;
        Ammo = ammo;
    }
}
