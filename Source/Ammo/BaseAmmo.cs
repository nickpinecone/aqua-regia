using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WaterGuns.Projectiles;

namespace WaterGuns.Ammo;

public abstract class BaseAmmo : ModItem
{
    public int Damage { get; set; }
    public Color Color { get; set; }

    public virtual void Apply(BaseProjectile baseProjectile)
    {
        baseProjectile.Projectile.damage += Damage;
    }

    public virtual void RuntimeHitNPC(NPC target, NPC.HitInfo hit)
    {
    }

    public virtual void RuntimeKill()
    {
    }
}