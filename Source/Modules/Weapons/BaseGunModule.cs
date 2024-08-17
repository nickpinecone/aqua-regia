namespace AquaRegia.Modules.Weapons;

public abstract class BaseGunModule
{
    protected BaseGunModule(BaseGun baseGun)
    {
        baseGun.AddModule(this);
    }
}
