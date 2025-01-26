using System;
using System.Collections.Generic;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace AquaRegia.UI;

class InterfaceSystem : ModSystem
{
    internal UserInterface _interface;
    internal GaugeState _gaugeState;
    private GameTime _lastUpdateUI;

    public override void Load()
    {
        base.Load();

        if (!Main.dedServ)
        {
            _interface = new UserInterface();
            _gaugeState = new GaugeState();
            _gaugeState.Activate();
            _interface.SetState(_gaugeState);
        }
    }

    public GaugeElement CreateGauge(string tooltip = "", int max = 0, int current = 0)
    {
        var gauge = new GaugeElement() {
            Tooltip = tooltip,
            Max = max,
            Current = current,
        };

        AddGauge(gauge);

        return gauge;
    }

    public void AddGauge(GaugeElement gauge)
    {
        _gaugeState.Add(gauge);
    }

    public void RemoveGauge(GaugeElement gauge)
    {
        _gaugeState.Remove(gauge);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        base.UpdateUI(gameTime);

        _lastUpdateUI = gameTime;
        if (_interface?.CurrentState != null)
        {
            _interface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        base.ModifyInterfaceLayers(layers);

        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("AquaRegia: Interface", delegate {
                              if (_lastUpdateUI != null && _interface?.CurrentState != null)
                              {
                                  _interface.Draw(Main.spriteBatch, _lastUpdateUI);
                              }
                              return true;
                          }, InterfaceScaleType.UI));
        }
    }
}
