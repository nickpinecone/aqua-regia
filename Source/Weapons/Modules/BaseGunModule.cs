namespace WaterGuns.Weapons.Modules;

public abstract class BaseGunModule
{
    protected BaseGun _baseGun;

    protected BaseGunModule(BaseGun baseGun)
    {
        _baseGun = baseGun;
        _baseGun.AddModule(this);
    }
}