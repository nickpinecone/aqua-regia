using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Projectiles;

namespace WaterGuns.Weapons.Modules;

class PropertyModule : BaseGunModule
{
    private float _inaccuracy;
    public float Inaccuracy
    {
        get { return _inaccuracy; }
        set { _inaccuracy = Math.Max(value, 0); }
    }

    public PropertyModule(BaseGun baseGun) : base(baseGun)
    {
    }

    public void SetDefaults()
    {
        _baseGun.Item.useAmmo = ItemID.BottledWater;
        _baseGun.Item.DamageType = DamageClass.Ranged;
    }

    public void SetProjectile<T>()
        where T : BaseProjectile
    {
        _baseGun.Item.shoot = ModContent.ProjectileType<T>();
    }

    public Vector2 ApplyInaccuracy(Vector2 velocity)
    {
        return velocity.RotatedByRandom(MathHelper.ToRadians(Inaccuracy));
    }
}