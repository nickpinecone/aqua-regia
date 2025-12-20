using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules.Items;

public class PropertyModule : IModule
{
    private readonly BaseItem _base;

    public PropertyModule(BaseItem item)
    {
        _base = item;
    }

    public PropertyModule Size(int width, int height)
    {
        _base.Item.width = width;
        _base.Item.height = height;
        return this;
    }

    public PropertyModule Damage(int damage, float knockBack, DamageClass type)
    {
        _base.Item.damage = damage;
        _base.Item.knockBack = knockBack;
        _base.Item.DamageType = type;
        return this;
    }

    public PropertyModule Ammo(int ammo)
    {
        _base.Item.ammo = ammo;
        return this;
    }

    /// <param name="ammo">For value use <see cref="ItemID"/></param>
    /// <param name="shootSpeed"></param>
    public PropertyModule Shoot<T>(int ammo, float shootSpeed)
        where T : BaseProjectile
    {
        _base.Item.noMelee = true;
        _base.Item.useAmmo = ammo;
        _base.Item.shootSpeed = shootSpeed;
        _base.Item.shoot = ModContent.ProjectileType<T>();
        return this;
    }

    /// <param name="useStyle">For use style use <see cref="ItemUseStyleID"/></param>
    /// <param name="useTime"></param>
    /// <param name="useAnimation"></param>
    /// <param name="autoReuse"></param>
    public PropertyModule UseStyle(int useStyle, int useTime, int useAnimation, bool autoReuse = true)
    {
        _base.Item.useTime = useTime;
        _base.Item.useAnimation = useAnimation;
        _base.Item.useStyle = useStyle;
        _base.Item.autoReuse = autoReuse;
        return this;
    }

    /// <param name="rare">For rarity use <see cref="ItemRarityID"/></param>
    public PropertyModule Rarity(int rare)
    {
        _base.Item.rare = rare;
        return this;
    }

    /// <param name="value">For value use Item.<see cref="Item.sellPrice"/> or Item.<see cref="Item.buyPrice"/></param>
    public PropertyModule Price(int value)
    {
        _base.Item.value = value;
        return this;
    }

    /// <param name="maxStack">For value can use Item.<see cref="Item.CommonMaxStack"/></param>
    /// <param name="consumable"></param>
    public PropertyModule MaxStack(int maxStack, bool consumable = false)
    {
        _base.Item.maxStack = maxStack;
        _base.Item.consumable = consumable;
        return this;
    }
}