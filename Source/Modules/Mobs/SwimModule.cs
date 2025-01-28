using Microsoft.Xna.Framework;
using AquaRegia.Utils;
using Terraria.ID;
using System;
using Terraria;

namespace AquaRegia.Modules.Mobs;

public class SwimModule : IModule
{
    public float DefaultGravity { get; private set; }
    public float Gravity { get; set; }
    public float DefaultChange { get; set; }
    public float GravityChange { get; set; }
    public float GravityCap { get; set; }

    public bool IsInWater(Rectangle rect)
    {
        foreach (var point in Collision.GetTilesIn(rect.TopLeft(), rect.BottomRight()))
        {
            var tile = TileHelper.GetTile(point);
            if (!(tile.LiquidAmount > 0 && tile.LiquidType == LiquidID.Water))
            {
                return false;
            }
        }

        return true;
    }

    public void SetGravity(float gravity = 0.01f, float gravityChange = 0.02f, float gravityCap = 8f)
    {
        DefaultGravity = gravity;
        Gravity = DefaultGravity;
        DefaultChange = gravityChange;
        GravityChange = DefaultChange;

        GravityCap = gravityCap;
    }

    public void ResetGravity()
    {
        Gravity = DefaultGravity;
        GravityChange = DefaultChange;
    }

    public Vector2 ApplyGravity(Rectangle rect, Vector2 velocity)
    {
        if (!IsInWater(rect))
        {
            if (velocity.Y < GravityCap)
            {
                Gravity += GravityChange;
                velocity.Y += Gravity;
                velocity.Y = MathF.Min(GravityCap, velocity.Y);
            }
        }
        else
        {
            if (Gravity > 0)
            {
                Gravity -= GravityChange;
                velocity.Y -= (Gravity / 2);

                if (Gravity < 0.1f)
                {
                    Gravity = 0f;
                    velocity.Y = 0;
                }
            }
        }

        return velocity;
    }
}
