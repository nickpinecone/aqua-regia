using System;
using AquaRegia.Players;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;

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

    public bool Pumped { get; private set; } = false;
    public bool Active { get; set; } = true;
    public Timer UpdateTimer { get; }

    public PumpModule(BaseGun baseGun, int updateTime = 20) : base(baseGun)
    {
        UpdateTimer = new Timer(updateTime);
    }

    public void ApplyToProjectile(BaseProjectile baseProjectile)
    {
    }

    public void Reset()
    {
        _pumpLevel = 0;
        UpdateTimer.Restart();
        Pumped = false;
    }

    public void DefaultUpdate(int amount = 1)
    {
        if (Active)
        {
            UpdateTimer.Update();

            if (UpdateTimer.Done && !Pumped)
            {
                _pumpLevel += amount;
                if (_pumpLevel >= _maxPumpLevel)
                {
                    Pumped = true;
                }
                else
                {
                    Pumped = false;
                    UpdateTimer.Restart();
                }
            }
        }
    }

    public void DirectUpdate(int amount = 1)
    {
        _pumpLevel += amount;

        if (_pumpLevel >= _maxPumpLevel)
        {
            Pumped = true;
        }
        else
        {
            Pumped = false;
        }
    }
}
