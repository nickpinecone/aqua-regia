namespace WaterGuns.Weapons.Modules;

public abstract class BaseGunModule
{
    protected BaseGunModule(BaseGun baseGun)
    {
        baseGun.AddModule(this);
    }
}
