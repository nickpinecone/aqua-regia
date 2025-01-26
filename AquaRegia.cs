using AquaRegia.Modules.Ammo;
using AquaRegia.Modules.Weapons;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia;

public class AquaRegia : Mod
{
    public class ProjectileSource : IEntitySource
    {
        public BaseWeapon? Weapon;
        public BaseAmmo? Ammo;

        public string? Context { get; set; }

        public ProjectileSource(ProjectileSource source) : this((IEntitySource)source)
        {
            Weapon = source.Weapon;
            Ammo = source.Ammo;
        }

        public ProjectileSource(IEntitySource source)
        {
            Context = source.Context;
        }
    }
}
