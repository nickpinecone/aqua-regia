using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Library.Helpers;

public static class ModHelper
{
    public static T SpawnProjectile<T>(IEntitySource source, Player player, Vector2 position, Vector2 velocity,
        int damage, float knockback)
        where T : ModProjectile
    {
        var type = ModContent.ProjectileType<T>();
        var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        return (T)proj.ModProjectile;
    }

    public static NPC? FindNearestNPC(Vector2 position, float radius, Func<NPC, bool>? canHome = null)
    {
        canHome ??= (_) => true;

        var nearestDistance = float.PositiveInfinity;
        NPC? nearestNpc = null;
        var detectRange = MathF.Pow(radius, 2);

        foreach (var target in Main.npc)
        {
            if (!target.CanBeChasedBy() || !canHome(target))
            {
                continue;
            }

            var distance = position.DistanceSQ(target.Center);

            if (distance <= detectRange && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestNpc = target;
            }
        }

        return nearestNpc;
    }

    public static Rectangle GetScreenTileRectangle()
    {
        var screenTilePosition = Main.screenPosition.ToTileCoordinates();

        var rect = new Rectangle(
            screenTilePosition.X - 5,
            screenTilePosition.Y - 5,
            (Main.ScreenSize.X / 16) + 10,
            (Main.ScreenSize.Y / 16) + 10
        );

        return rect;
    }
}