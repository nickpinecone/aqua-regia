using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

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

    public void DefaultAmmo()
    {
        _baseGun.Item.useAmmo = ItemID.BottledWater;
    }

    public Vector2 ApplyInaccuracy(Vector2 vector)
    {
        return vector.RotatedByRandom(MathHelper.ToRadians(Inaccuracy));
    }
}