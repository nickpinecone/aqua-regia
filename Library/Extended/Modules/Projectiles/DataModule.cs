using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Library.Modules.Projectiles;

public class DataModule<TSource, TPlayer> : IModule, IProjectileRuntime
    where TSource : IEntitySource
    where TPlayer : ModPlayer
{
    public TSource Source { get; private set; } = default!;
    public TPlayer Player { get; private set; } = null!;

    public void RuntimeOnSpawn(BaseProjectile baseProjectile, IEntitySource source)
    {
        Source = (TSource)source;
        Player = baseProjectile.Owner.GetModPlayer<TPlayer>();
    }
}