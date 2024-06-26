using System;
using WaterGuns.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Modules;

public class PumpModule : BaseGunModule
{
    private int _pumpLevel;
    public int PumpLevel
    {
        get { return _pumpLevel; }
        set { _pumpLevel = Math.Max(0, value); }
    }

    private int _maxPumpLevel;
    public int MaxPumpLevel
    {
        get { return _maxPumpLevel; }
        set { _maxPumpLevel = Math.Max(0, value); }
    }
    public bool Pumped { get; private set; }

    public Timer PumpTimer { get; }

    public PumpModule(BaseGun baseGun, int waitTime) : base(baseGun)
    {
        PumpTimer = new Timer(waitTime);
    }

    public void ApplyToProjectile(BaseProjectile baseProjectile)
    {
        baseProjectile.Projectile.scale *= 1.5f;
    }

    public void Reset()
    {
        _pumpLevel = 0;
        PumpTimer.Restart();
        Pumped = false;
    }

    public void DefaultUpdate()
    {
        if (PumpTimer.Done && !Pumped)
        {
            _pumpLevel += 1;
            if (_pumpLevel >= _maxPumpLevel)
            {
                Pumped = true;
            }
            else
            {
                PumpTimer.Restart();
            }
        }
    }

    public void DirectUpdate()
    {
        _pumpLevel += 1;
        if (_pumpLevel >= _maxPumpLevel)
        {
            Pumped = true;
        }
    }
}
