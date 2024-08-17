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

    public void SetDefaults(BaseGun baseGun)
    {
        baseGun.Item.width = 0;
        baseGun.Item.height = 0;
        baseGun.Item.damage = 0;
        baseGun.Item.knockBack = 0f;

        baseGun.Item.useTime = 0;
        baseGun.Item.useAnimation = 0;
        baseGun.Item.shootSpeed = 0f;

        baseGun.Item.maxStack = 1;
        baseGun.Item.noMelee = true;
        baseGun.Item.autoReuse = true;

        baseGun.Item.rare = ItemRarityID.White;
        baseGun.Item.value = Item.sellPrice(0, 0, 0, 0);

        baseGun.Item.useStyle = ItemUseStyleID.Shoot;
        baseGun.Item.useAmmo = ModContent.ItemType<BottledWater>();
        baseGun.Item.DamageType = DamageClass.Ranged;
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
