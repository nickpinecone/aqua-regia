namespace WaterGuns.Projectiles.Modules;

public abstract class BaseProjectileModule
{
    protected BaseProjectile _baseProjectile;

    protected BaseProjectileModule(BaseProjectile baseProjectile)
    {
        _baseProjectile = baseProjectile;
        _baseProjectile.AddModule(this);
    }
}