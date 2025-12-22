using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class PropertyModule : IModule
{
    private BaseProjectile _base = null!;

    public PropertyModule Set(BaseProjectile projectile)
    {
        _base = projectile;
        return this;
    }

    public PropertyModule Size(int width, int height)
    {
        _base.Projectile.width = width;
        _base.Projectile.height = height;
        return this;
    }

    public PropertyModule Damage(DamageClass damageType, int penetrate, int critChance = 0)
    {
        _base.Projectile.DamageType = damageType;
        _base.Projectile.penetrate = penetrate;
        _base.Projectile.CritChance = critChance;
        return this;
    }

    public PropertyModule TimeLeft(int timeLeft)
    {
        _base.Projectile.timeLeft = timeLeft;
        return this;
    }

    public PropertyModule Friendly(bool friendly, bool hostile)
    {
        _base.Projectile.friendly = friendly;
        _base.Projectile.hostile = hostile;
        return this;
    }

    public PropertyModule Alpha(int alpha)
    {
        _base.Projectile.alpha = alpha;
        return this;
    }

    public PropertyModule TileCollide(bool tileCollide)
    {
        _base.Projectile.tileCollide = tileCollide;
        return this;
    }
}