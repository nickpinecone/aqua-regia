using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules.Items;

public class PropertyModule : IModule
{
    private BaseItem _item;

    public PropertyModule(BaseItem item)
    {
        _item = item;
    }

    public PropertyModule Size(int width, int height)
    {
        _item.Item.width = width;
        _item.Item.height = height;
        return this;
    }

    public PropertyModule Damage(int damage, float knockBack, DamageClass type)
    {
        _item.Item.damage = damage;
        _item.Item.knockBack = knockBack;
        _item.Item.DamageType = type;
        return this;
    }

    /// <param name="rare">For rarity use <see cref="ItemRarityID"/></param>
    public PropertyModule Rarity(int rare)
    {
        _item.Item.rare = rare;
        return this;
    }

    /// <param name="value">For value use Item.<see cref="Item.sellPrice"/> or Item.<see cref="Item.buyPrice"/></param>
    public PropertyModule Price(int value)
    {
        _item.Item.value = value;
        return this;
    }

    /// <param name="maxStack">For value can use Item.<see cref="Item.CommonMaxStack"/></param>
    /// <param name="consumable"></param>
    public PropertyModule MaxStack(int maxStack, bool consumable = false)
    {
        _item.Item.maxStack = maxStack;
        _item.Item.consumable = consumable;
        return this;
    }

    /// <param name="ammo">For value use <see cref="ItemID"/></param>
    public PropertyModule Ammo(int ammo)
    {
        _item.Item.ammo = ammo;
        return this;
    }

    public void Projectile<T>()
        where T : BaseProjectile
    {
        _item.Item.shoot = ModContent.ProjectileType<T>();
    }
}