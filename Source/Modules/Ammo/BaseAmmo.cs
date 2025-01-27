using AquaRegia.Modules.Projectiles;
using Terraria.ModLoader;

namespace AquaRegia.Modules.Ammo;

public abstract class BaseAmmo : ModItem
{
    public virtual void ApplyToProjectile(BaseProjectile baseProjectile)
    {
    }
}
