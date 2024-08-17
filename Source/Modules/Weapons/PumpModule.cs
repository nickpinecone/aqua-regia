using System;
using AquaRegia.Utils;

namespace AquaRegia.Modules.Weapons;

public class PumpModule : BaseGunModule
{
    private int _pumpLevel;
    public int PumpLevel
    {
        get {
            return _pumpLevel;
        }
        set {
            _pumpLevel = Math.Max(0, value);
        }
    }

    private int _maxPumpLevel;
    public int MaxPumpLevel
    {
        get {
            return _maxPumpLevel;
        }
        set {
            _maxPumpLevel = Math.Max(0, value);
        }
    }
    public bool Pumped { get; private set; }

    public Timer PumpTimer { get; }

    public PumpModule(BaseGun baseGun, int pumpTime = 20) : base(baseGun)
    {
        PumpTimer = new Timer(pumpTime, baseGun);
    }

    public void ApplyToProjectile(BaseProjectile baseProjectile)
    {
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
