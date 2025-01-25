using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Ammo;

namespace AquaRegia.Modules.Weapons;

public class PropertyModule : BaseGunModule
{
    private float _inaccuracy;
    public float Inaccuracy
    {
        get {
            return _inaccuracy;
        }
        set {
            _inaccuracy = Math.Max(value, 0);
        }
    }

    public PropertyModule(BaseGun baseGun) : base(baseGun)
    {
    }

    public void SetDefaults(BaseGun baseGun, int width = 0, int height = 0, int damage = 0, float knockBack = 0f,
                            float inaccuracy = 0f, int useTime = 0, int useAnimation = 0, float shootSpeed = 0f,
                            int rarity = ItemRarityID.White, int sellPrice = 0, int maxStack = 1, bool noMelee = true,
                            bool autoReuse = true, int useStyle = ItemUseStyleID.Shoot, int? useAmmo = null,
                            DamageClass? damageType = null)
    {
        baseGun.Item.width = width;
        baseGun.Item.height = height;
        baseGun.Item.damage = damage;
        baseGun.Item.knockBack = knockBack;
        _inaccuracy = inaccuracy;

        baseGun.Item.useTime = useTime;
        baseGun.Item.useAnimation = useAnimation;
        baseGun.Item.shootSpeed = shootSpeed;

        baseGun.Item.rare = rarity;
        baseGun.Item.value = sellPrice;

        baseGun.Item.maxStack = maxStack;
        baseGun.Item.noMelee = noMelee;
        baseGun.Item.autoReuse = autoReuse;

        baseGun.Item.useStyle = useStyle;
        baseGun.Item.useAmmo = useAmmo ?? ModContent.ItemType<BottledWater>();
        baseGun.Item.DamageType = damageType ?? DamageClass.Ranged;
    }

    public void SetProjectile<T>(BaseGun baseGun)
        where T : BaseProjectile
    {
        baseGun.Item.shoot = ModContent.ProjectileType<T>();
    }

    public Vector2 ApplyInaccuracy(Vector2 velocity)
    {
        return velocity.RotatedByRandom(MathHelper.ToRadians(Inaccuracy));
    }
}
