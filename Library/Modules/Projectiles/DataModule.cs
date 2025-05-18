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

public class ProjectileSource : IEntitySource
{
    public BaseProjectile? Parent { get; }
    public string? Context { get; set; }

    public ProjectileSource(ProjectileSource source)
        : this(source, source.Parent)
    {
    }

    public ProjectileSource(BaseProjectile projectile)
        : this(projectile.Projectile.GetSource_FromThis(), projectile)
    {
    }

    public ProjectileSource()
        : this(Entity.GetSource_None(), null)
    {
    }

    private ProjectileSource(IEntitySource? source, BaseProjectile? projectile = null)
    {
        Context = source?.Context;
        Parent = projectile;
    }
}