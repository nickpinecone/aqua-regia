using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Sources;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Projectiles.Explosion;

public class ExplosionSource : ProjectileSource
{
    public readonly DamageClass DamageType;
    public readonly int Radius;
    public readonly int Damage;
    public readonly float KnockBack;
    public readonly int CritChance;

    public ExplosionSource(BaseProjectile projectile, DamageClass damageType, int radius, int damage, float knockBack,
        int critChance) : base(projectile)
    {
        DamageType = damageType;
        Radius = radius;
        Damage = damage;
        KnockBack = knockBack;
        CritChance = critChance;
    }
}