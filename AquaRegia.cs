using AquaRegia.Modules.Ammo;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Modules.Weapons;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia;

public class AquaRegia : Mod
{
    public class WeaponWithAmmoSource : IEntitySource
    {
        public BaseWeapon Weapon { get; private set; }
        public BaseAmmo? Ammo { get; private set; }

        public string? Context { get; set; }

        public WeaponWithAmmoSource(WeaponWithAmmoSource source)
            : this((IEntitySource)source, source.Weapon, source.Ammo)
        {
        }

        public WeaponWithAmmoSource(EntitySource_ItemUse_WithAmmo source, BaseWeapon weapon)
            : this((IEntitySource)source, weapon, (BaseAmmo)ModContent.GetModItem(source.AmmoItemIdUsed))
        {
        }

        public WeaponWithAmmoSource(IEntitySource source, BaseWeapon weapon, BaseAmmo? ammo = null)
        {
            Context = source.Context;
            Weapon = weapon;
            Ammo = ammo;
        }
    }

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
}
