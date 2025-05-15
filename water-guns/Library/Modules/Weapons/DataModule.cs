using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace WaterGuns.Library.Modules.Weapons;

public class DataModule<TPlayer> : IModule
    where TPlayer : ModPlayer
{
    private TPlayer? _player = null;

    public TPlayer GetPlayer(Player player)
    {
        return _player ??= player.GetModPlayer<TPlayer>();
    }
}

public class WeaponWithAmmoSource : IEntitySource
{
    public BaseWeapon Weapon { get; private set; }
    public BaseAmmo? Ammo { get; private set; }
    public string? Context { get; set; }

    public WeaponWithAmmoSource(WeaponWithAmmoSource source)
        : this(source, source.Weapon, source.Ammo)
    {
    }

    public WeaponWithAmmoSource(EntitySource_ItemUse_WithAmmo source, BaseWeapon weapon)
        : this(source, weapon, (BaseAmmo)ModContent.GetModItem(source.AmmoItemIdUsed))
    {
    }
    
    public WeaponWithAmmoSource(BaseWeapon weapon)
        : this(weapon.Item.GetSource_FromThis(), weapon, null)
    {
    }

    private WeaponWithAmmoSource(IEntitySource source, BaseWeapon weapon, BaseAmmo? ammo = null)
    {
        Context = source.Context;
        Weapon = weapon;
        Ammo = ammo;
    }
}