using WaterGuns.Ammo;
using WaterGuns.Modules.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace WaterGuns.Modules.Ammo;

public abstract class BaseAmmo : ModItem
{
    protected void SetProperties(int damage = 0, float knockBack = 0f, int rarity = 0, int sellPrice = 0,
                                 int width = 14, int height = 28, int? maxStack = null, bool consumable = true,
                                 int? ammo = null, DamageClass? damageType = null)
    {
        Item.damage = damage;
        Item.knockBack = knockBack;

        Item.rare = rarity;
        Item.value = sellPrice;

        Item.width = width;
        Item.height = height;

        Item.maxStack = maxStack ?? Item.CommonMaxStack;
        Item.consumable = consumable;
        Item.ammo = ammo ?? ModContent.ItemType<BottledWater>();
        Item.DamageType = damageType ?? DamageClass.Ranged;
    }

    public virtual void ApplyToProjectile(BaseProjectile projectile)
    {
    }
}
