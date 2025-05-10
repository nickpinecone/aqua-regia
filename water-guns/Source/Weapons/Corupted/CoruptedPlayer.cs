using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WaterGuns.Players;
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
    private Timer _updateTimer = new(20);

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

    public void SpawnWorms()
    {
        InitializeGauge();

        var amount = Main.rand.Next(2, 4);

        var screenBottom = Main.MouseWorld;
        screenBottom.Y += Main.ScreenSize.Y - Main.MouseScreen.Y + 64f;
        var velocity = Vector2.UnitY * 16f;

        for (var i = 0; i < amount; i++)
        {
            var position = screenBottom - new Vector2(Main.rand.Next(-64, 64), 0f);

            var worm = Helper.SpawnProjectile<CoruptedWormHead>(Projectile.GetSource_NaturalSpawn(), Player, position,
                velocity,
                18, 1f);

            _coruptedWorms.Add(worm);
        }
    }

    public bool AnyWorms()
    {
        return _coruptedWorms.Count != 0;
    }

    public override void PreUpdate()
    {
        base.PreUpdate();

        if (_gauge != null)
        {
            foreach (var worm in _coruptedWorms) worm.Projectile.timeLeft = 2;

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