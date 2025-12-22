using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Sources;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Projectiles.Explosion;

public class ExplosionSource : ProjectileSource
{
    public readonly DamageClass DamageType;
    public readonly int Radius;
    public readonly int CritChance;

    public ExplosionSource(BaseProjectile projectile, DamageClass damageType, int radius, int critChance) :
        base(projectile)
    {
        DamageType = damageType;
        Radius = radius;
        CritChance = critChance;
    }
}