using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Library.Modules;

// TODO Maybe remake this to IComposite as well
// Just for future proofing
public abstract class BaseAmmo : ModItem, IComposite<IItemRuntime>
{
    public Dictionary<Type, IModule> Modules { get; } = [];
    public List<IItemRuntime> RuntimeModules { get; } = [];

    // TODO this a PropertyModule
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

        // TODO would be cool to have something like SetAmmoDefaults
        // and for other types of common items
        Item.maxStack = maxStack ?? Item.CommonMaxStack;
        Item.consumable = consumable;
        Item.ammo = ammo ?? ItemID.BottledWater;
        Item.DamageType = damageType ?? DamageClass.Ranged;
    }

    public virtual void ApplyToProjectile(BaseProjectile projectile)
    {
    }
}