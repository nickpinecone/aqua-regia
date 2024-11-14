namespace AquaRegia.Modules.Weapons;

public abstract class BaseGunModule
{
    protected BaseGunModule(IComposite<BaseGunModule> baseGun)
    {
        baseGun.AddModule(this);
    }
}
