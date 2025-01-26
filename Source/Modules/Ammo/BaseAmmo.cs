using AquaRegia.Modules.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Modules.Ammo;

public abstract class BaseAmmo : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 25;
    }

    public override void SetDefaults()
    {
    }

    public virtual void ApplyToProjectile(BaseProjectile baseProjectile)
    {
    }
}
