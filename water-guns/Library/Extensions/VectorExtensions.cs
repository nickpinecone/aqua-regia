using System;
using Microsoft.Xna.Framework;

namespace WaterGuns.Library.Extensions;

public static class VectorExtensions
{
    public static float AngleBetween(this Vector2 u, Vector2 v)
    {
        var dot = u.X * v.X + u.Y * v.Y;
        var det = u.X * v.Y - u.Y * v.X;

        return MathF.Atan2(det, dot);
    }

    public static float AngleOne(this Vector2 u, Vector2 v)
    {
        var angle = u.AngleBetween(v);
        
        if (angle <= 0)
        {
            angle = MathHelper.TwoPi + angle;
        }

        return angle;
    }
}