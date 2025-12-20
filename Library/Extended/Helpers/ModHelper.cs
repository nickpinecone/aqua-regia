using System;
using AquaRegia.Library.Extended.Projectiles.Explosion;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Helpers;

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

    public static ExplosionProjectile SpawnExplosion(ExplosionSource source, Player player, Vector2 position)
    {
        var type = ModContent.ProjectileType<ExplosionProjectile>();
        var proj = Projectile.NewProjectileDirect(source, position, Vector2.Zero, type, source.Damage, source.KnockBack,
            player.whoAmI);
        return (ExplosionProjectile)proj.ModProjectile;
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
}