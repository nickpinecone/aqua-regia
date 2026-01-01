using AquaRegia.Library.Extended.Fluent;
using AquaRegia.Library.Extended.Modules.Projectiles;
using AquaRegia.Library.Extended.Sources;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules.Items;

public class RecallModule<T> : IModule, IItemRuntime
    where T : BaseProjectile
{
    public float ThrowSpeed { get; set; }

    public void SetDefaults(float throwSpeed)
    {
        ThrowSpeed = throwSpeed;
    }

    public void Throw(BaseItem item, Player player)
    {
        new ProjectileSpawner<T>()
            .Context(new WeaponWithAmmoSource(item), player)
            .Damage(item.Item.damage, item.Item.knockBack)
            .Position(player.Center)
            .Velocity((Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * ThrowSpeed)
            .Spawn();
    }

    public void Recall()
    {
        foreach (var proj in Main.ActiveProjectiles)
        {
            if (proj.ModProjectile is T thrown && thrown.Composite.TryGetModule<RecallModule>(out var recall))
            {
                recall.Recall();
            }
        }
    }

    public void ThrowOrRecall(BaseItem item, Player player)
    {
        if (player.ownedProjectileCounts[item.Item.shoot] < 1 &&
            player.ownedProjectileCounts[ModContent.ProjectileType<T>()] < 1)
        {
            Throw(item, player);
        }
        else
        {
            Recall();
        }
    }

    public bool RuntimeCanUseItem(BaseItem item, Player player)
    {
        return player.ownedProjectileCounts[ModContent.ProjectileType<T>()] < 1;
    }

    public void RuntimeAltUseAlways(BaseItem item, Player player)
    {
        ThrowOrRecall(item, player);
    }
}