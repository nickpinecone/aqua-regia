using AquaRegia.Library.Extended.Helpers;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Extensions;

public static class ProjectileExtensions
{
    public static bool IsTileCollide(this Projectile projectile)
    {
        var startX = projectile.position.X;
        var startY = projectile.position.Y;

        for (var dx = 0; dx < projectile.width; dx += 16)
        {
            for (var dy = 0; dy < projectile.height; dy += 16)
            {
                if (TileHelper.IsSolid(new Vector2(startX + dx, startY + dy)))
                {
                    return true;
                }
            }
        }

        return false;
    }
}