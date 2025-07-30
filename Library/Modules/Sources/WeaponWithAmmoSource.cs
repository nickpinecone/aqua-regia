using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Library.Modules.Sources;

public class WeaponWithAmmoSource : IEntitySource
{
    public BaseItem Item { get; private set; }
    public BaseAmmo? Ammo { get; private set; }
    public string? Context { get; set; }

    public WeaponWithAmmoSource(WeaponWithAmmoSource source)
        : this(source, source.Item, source.Ammo)
    {
    }

    public WeaponWithAmmoSource(EntitySource_ItemUse_WithAmmo source, BaseItem item)
        : this(source, item, (BaseAmmo)ModContent.GetModItem(source.AmmoItemIdUsed))
    {
    }

    public WeaponWithAmmoSource(BaseItem item)
        : this(item.Item.GetSource_FromThis(), item, null)
    {
    }

    private WeaponWithAmmoSource(IEntitySource source, BaseItem item, BaseAmmo? ammo = null)
    {
        Context = source.Context;
        Item = item;
        Ammo = ammo;
    }
}