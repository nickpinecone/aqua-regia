using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Ammo;
using AquaRegia.Modules.Projectiles;

namespace AquaRegia.Modules.Weapons;

public class PropertyModule : IModule
{
    private float _inaccuracy;
    public float Inaccuracy
    {
        get
        {
            return _inaccuracy;
        }
        set
        {
            _inaccuracy = Math.Max(value, 0);
        }
    }

    public void SetProperties(BaseWeapon weapon, int width = 0, int height = 0, int damage = 0, float knockBack = 0f,
                              float inaccuracy = 0f, int useTime = 0, int useAnimation = 0, float shootSpeed = 0f,
                              int rarity = ItemRarityID.White, int sellPrice = 0, int maxStack = 1, bool noMelee = true,
                              bool autoReuse = true, int useStyle = ItemUseStyleID.Shoot, int? useAmmo = null,
                              DamageClass? damageType = null)
    {
        weapon.Item.width = width;
        weapon.Item.height = height;
        weapon.Item.damage = damage;
        weapon.Item.knockBack = knockBack;
        _inaccuracy = inaccuracy;

        weapon.Item.useTime = useTime;
        weapon.Item.useAnimation = useAnimation;
        weapon.Item.shootSpeed = shootSpeed;

        weapon.Item.rare = rarity;
        weapon.Item.value = sellPrice;

        weapon.Item.maxStack = maxStack;
        weapon.Item.noMelee = noMelee;
        weapon.Item.autoReuse = autoReuse;

        weapon.Item.useStyle = useStyle;
        weapon.Item.useAmmo = useAmmo ?? ModContent.ItemType<BottledWater>();
        weapon.Item.DamageType = damageType ?? DamageClass.Ranged;
    }

    public void SetProjectile<T>(BaseWeapon baseGun)
        where T : BaseProjectile
    {
        baseGun.Item.shoot = ModContent.ProjectileType<T>();
    }

    public Vector2 ApplyInaccuracy(Vector2 velocity)
    {
        return velocity.RotatedByRandom(MathHelper.ToRadians(Inaccuracy));
    }
}
