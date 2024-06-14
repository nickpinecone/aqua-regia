using Terraria.DataStructures;
using Terraria.ModLoader;
using WaterGuns.Ammo;
using WaterGuns.Weapons;

namespace WaterGuns;

public class WaterGuns : Mod
{
    public class ProjectileSource : IEntitySource
    {
        public BaseGun Weapon;
        public BaseAmmo Ammo;

        public string Context { get; set; }
        public ProjectileSource(IEntitySource source)
        {
            Context = source.Context;
        }
    }
}