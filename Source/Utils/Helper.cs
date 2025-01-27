using System;
using System.Collections.Generic;
using AquaRegia.Modules.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Utils;

public static class Helper
{
    public static float AngleBetween(Vector2 u, Vector2 v)
    {
        var dot = u.X * v.X + u.Y * v.Y;
        var det = u.X * v.Y - u.Y * v.X;

        return MathF.Atan2(det, dot);
    }

    public static float AngleOne(Vector2 u, Vector2 v)
    {
        var angle = AngleBetween(u, v);

        if (angle <= 0)
        {
            angle = MathHelper.TwoPi + angle;
        }

        return angle;
    }

    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = Main.rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Vector2 ToVector2I(Vector2 vector)
    {
        return new Vector2((int)vector.X, (int)vector.Y);
    }

    public static T SpawnProjectile<T>(IEntitySource source, Player player, Vector2 position, Vector2 velocity,
                                       int damage, float knockback)
        where T : BaseProjectile
    {
        var type = ModContent.ProjectileType<T>();
        var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        return (T)proj.ModProjectile;
    }

    public static NPC? FindNearestNPC(Vector2 position, float radius, Func<NPC, bool>? canHome = null)
    {
        canHome ??= (_) => true;

        float nearestDistance = float.PositiveInfinity;
        NPC? nearestNpc = null;
        float detectRange = MathF.Pow(radius, 2);

        for (int i = 0; i < Main.npc.Length; i++)
        {
            NPC target = Main.npc[i];

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
}
