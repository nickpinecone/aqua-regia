using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using AquaRegia.Modules;
using AquaRegia.Modules.Weapons;
using AquaRegia.Utils;
using System.Collections.Generic;
using ReLogic.Content;

namespace AquaRegia.UI;

class GaugeState : UIState
{
    GaugeElement _pumpGauge;
    List<GaugeElement> _gauges = new();

    private int _halign = 97;

    public override void OnInitialize()
    {
        GaugeElement.Texture = ModContent.Request<Texture2D>(TexturesPath.UI + "GaugeFrame");

        _pumpGauge = new GaugeElement();
        _pumpGauge.Tooltip = "Pump Gauge";
        _pumpGauge.HAlign = 0.99f;
        _pumpGauge.VAlign = 0.98f;

        Append(_pumpGauge);
    }

    public void Add(GaugeElement gauge)
    {
        if (!_gauges.Contains(gauge))
        {
            gauge.HAlign = _halign / (float)100;
            _halign -= 2;
            gauge.VAlign = 0.98f;

            _gauges.Add(gauge);
            Append(gauge);
        }
    }

    public void Remove(GaugeElement gauge)
    {
        if (_gauges.Contains(gauge))
        {
            _gauges.Remove(gauge);
            RemoveChild(gauge);

            foreach (var child in _gauges)
            {
                if (child.HAlign < gauge.HAlign)
                {
                    child.HAlign += 0.02f;
                }
            }

            _halign += 2;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!(Main.LocalPlayer.HeldItem.ModItem is BaseGun baseGun && baseGun.HasModule<PumpModule>()))
        {
            _pumpGauge.Hidden = true;
        }
        else
        {
            _pumpGauge.Hidden = false;
            var pump = ((BaseGun)Main.LocalPlayer.HeldItem.ModItem).GetModule<PumpModule>();

            _pumpGauge.Active = pump.Active;
            _pumpGauge.Current = pump.PumpLevel;
            _pumpGauge.Max = pump.MaxPumpLevel;
        }

        base.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
