using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using WaterGuns.Sources;
using WaterGuns.UI;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Corupted;

public class CoruptedSource : WeaponWithAmmoSource
{
    public int SplitCount = 0;

    public CoruptedSource(WeaponWithAmmoSource source, int splitCount) : base(source)
    {
        SplitCount = splitCount;
    }
}

public class CoruptedPlayer : ModPlayer
{
    private List<CoruptedWormHead> _coruptedWorms = new();
    private GaugeElement? _gauge = null;
    private Timer _updateTimer = new Timer(20);

    private void InitializeGauge()
    {
        if (_gauge is null)
        {
            _gauge = ModContent.GetInstance<InterfaceSystem>().CreateGauge("Corupted Gauge", 60, 60);
            
            _gauge.ColorBorderFull = Color.White;
            _gauge.ColorA = Color.DarkMagenta;
            _gauge.ColorB = Color.MediumPurple;
        }
    }

    public bool AnyWorms()
    {
        return _coruptedWorms.Count != 0;
    }

    public void AddWorm(CoruptedWormHead coruptedWorm)
    {
        InitializeGauge();
        _coruptedWorms.Add(coruptedWorm);
    }

    public override void PreUpdate()
    {
        base.PreUpdate();

        if (_gauge != null)
        {
            foreach (var worm in _coruptedWorms)
            {
                worm.Projectile.timeLeft = 2;
            }

            _updateTimer.Update();

            if (_updateTimer.Done)
            {
                _updateTimer.Restart();

                _gauge.Current -= 1;

                if (_gauge.Current <= 0)
                {
                    ModContent.GetInstance<InterfaceSystem>().RemoveGauge(_gauge);
                    _gauge = null;
                    _coruptedWorms.Clear();
                }
            }
        }
    }
}