using Terraria.DataStructures;
using WaterGuns.Modules.Projectiles;

namespace WaterGuns.Sources;

public class ProjectileSource : IEntitySource
{
    public BaseProjectile? Parent { get; private set; }

    public string? Context { get; set; }

    public ProjectileSource(ProjectileSource source) : this((IEntitySource)source, source.Parent)
    {
    }

    public ProjectileSource(IEntitySource source, BaseProjectile? projectile = null)
    {
        Context = source.Context;
        Parent = projectile;
    }
}