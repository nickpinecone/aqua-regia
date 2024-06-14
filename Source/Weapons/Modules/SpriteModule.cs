
using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Weapons.Modules;

class SpriteModule : BaseGunModule
{
    public Vector2 Offset { get; set; }
    public Vector2 Shift { get; set; }

    public Vector2 HoldoutOffset { get; set; }

    public SpriteModule(BaseGun baseGun) : base(baseGun)
    {
    }

    public Vector2 ApplyOffset(Vector2 position, Vector2 velocity)
    {
        var offset = new Vector2(position.X + velocity.X * Offset.X, position.Y + velocity.Y * Offset.Y);

        return offset + Shift;
    }
}