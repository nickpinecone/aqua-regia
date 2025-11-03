using Terraria;
using Terraria.DataStructures;

namespace AquaRegia.Library.Extended.Modules.Sources;

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