using AquaRegia.Modules.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Utils;

public static class Helper
{
    public static T SpawnProjectile<T>(IEntitySource source, Player player, Vector2 position, Vector2 velocity,
                                       int damage, float knockback)
        where T : BaseProjectile
    {
        var type = ModContent.ProjectileType<T>();
        var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        return (T)proj.ModProjectile;
    }
}
