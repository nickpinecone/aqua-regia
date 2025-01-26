using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia;

public class AquaRegia : Mod
{
    public class ProjectileSource : IEntitySource
    {
        public string? Context { get; set; }

        public ProjectileSource(IEntitySource source)
        {
            Context = source.Context;
        }
    }
}
