using Terraria.DataStructures;
using Terraria.ModLoader;
using AquaRegia.Modules;

namespace AquaRegia;

public class AquaRegia : Mod
{
    public class ProjectileSource : IEntitySource
    {
        public BaseGun? Weapon;
        public BaseAmmo? Ammo;

        public void Inherit(ProjectileSource original)
        {
            Context = original.Context;
            Weapon = original.Weapon;
            Ammo = original.Ammo;
        }

        public string? Context { get; set; }
        public ProjectileSource(IEntitySource source)
        {
            Context = source.Context;
        }
    }
}
