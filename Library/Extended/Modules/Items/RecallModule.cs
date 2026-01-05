using AquaRegia.Library.Extended.Fluent;
using AquaRegia.Library.Extended.Modules.Projectiles;
using AquaRegia.Library.Extended.Sources;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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

    public void ThrowProjectile(BaseItem baseItem, Player player)
    {
        SoundEngine.PlaySound(SoundID.Item1);
        
        new ProjectileSpawner<T>()
            .Context(new WeaponWithAmmoSource(baseItem), player)
            .Damage(baseItem.Item.damage, baseItem.Item.knockBack)
            .Position(player.Center)
            .Velocity((Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * ThrowSpeed)
            .Spawn();
    }

    public void RecallAll()
    {
        SoundEngine.PlaySound(SoundID.Item7);
        
        foreach (var proj in Main.ActiveProjectiles)
        {
            if (proj.ModProjectile is T thrown && thrown.Composite.TryGetModule<RecallModule>(out var recall))
            {
                recall.Recall(proj);
            }
        }
    }

    public void ThrowOrRecall(BaseItem baseItem, Player player)
    {
        if (player.ownedProjectileCounts[baseItem.Item.shoot] < 1 &&
            player.ownedProjectileCounts[ModContent.ProjectileType<T>()] < 1)
        {
            ThrowProjectile(baseItem, player);
        }
        else
        {
            RecallAll();
        }
    }

    public bool RuntimeCanUseItem(BaseItem baseItem, Player player)
    {
        return player.ownedProjectileCounts[ModContent.ProjectileType<T>()] < 1;
    }

    public void RuntimeAltUseAlways(BaseItem baseItem, Player player)
    {
        ThrowOrRecall(baseItem, player);
    }
}