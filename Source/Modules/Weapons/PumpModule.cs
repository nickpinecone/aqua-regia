using System;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;

namespace AquaRegia.Modules.Weapons;

public class PumpModule : BaseGunModule
{
    private int _pumpLevel;
    public int PumpLevel
    {
        get
        {
            return _pumpLevel;
        }
        set
        {
            _pumpLevel = Math.Max(0, value);
        }
    }

    private int _maxPumpLevel;
    public int MaxPumpLevel
    {
        get
        {
            return _maxPumpLevel;
        }
        set
        {
            _maxPumpLevel = Math.Max(0, value);
        }
    }
    public bool Pumped { get; private set; }

    public Color ColorA { get; set; }
    public Color ColorB { get; set; }

    public bool DoDecrease { get; private set; }
    public Timer UpdateTimer { get; }
    public Timer DecreaseTimer { get; }

    public PumpModule(BaseGun baseGun, int updateTime = 20, int decraseTime = 20) : base(baseGun)
    {
        ColorA = Color.Blue;
        ColorB = Color.Cyan;

        UpdateTimer = new Timer(updateTime, baseGun);
        DecreaseTimer = new Timer(decraseTime);
        baseGun.AddInventoryTimer(DecreaseTimer);
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

    public void StartDecrease()
    {
        DoDecrease = true;
        Pumped = false;
        DecreaseTimer.Restart();
    }

    public void DefaultUpdate(int amount = 1)
    {
        if (UpdateTimer.Done && !Pumped)
        {
            _pumpLevel += amount;
            if (_pumpLevel >= _maxPumpLevel)
            {
                Pumped = true;
            }
            else
            {
                UpdateTimer.Restart();
            }
        }
    }

    public void DefaultDecrease(int amount = 1)
    {
        if (DoDecrease && DecreaseTimer.Done)
        {
            _pumpLevel -= amount;
            if (_pumpLevel == 0)
            {
                DoDecrease = false;
            }
            else
            {
                DecreaseTimer.Restart();
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
    }
}
