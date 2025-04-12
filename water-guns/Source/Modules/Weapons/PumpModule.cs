using System;
using WaterGuns.Utils;

namespace WaterGuns.Modules.Weapons;

public class PumpModule : IModule
{
    private int _pumpLevel;
    public int PumpLevel
    {
        get
        {
            return _pumpLevel;
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

    public bool Pumped { get; private set; } = false;
    public bool Active { get; set; } = true;
    public Timer UpdateTimer { get; private set; }

    public PumpModule()
    {
        UpdateTimer = new Timer(0);
    }

    public void SetDefaults(int maxPumpLevel, Timer? updateTimer = null)
    {
        MaxPumpLevel = maxPumpLevel;
        UpdateTimer = updateTimer ?? new Timer(20);
    }

    public void Reset()
    {
        _pumpLevel = 0;
        UpdateTimer.Restart();
        Pumped = false;
    }

    public void Update(int amount = 1)
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
