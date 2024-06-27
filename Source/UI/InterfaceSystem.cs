using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace WaterGuns.UI;

class InterfaceSystem : ModSystem
{
    internal UserInterface _interface;
    internal PumpBar _pumpBar;
    private GameTime _lastUpdateUI;

    public override void Load()
    {
        base.Load();

        _interface = new UserInterface();
        _pumpBar = new PumpBar();
        _pumpBar.Activate();
        _interface.SetState(_pumpBar);
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
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("WaterGuns: Interface", delegate
            {
                if (_lastUpdateUI != null && _interface?.CurrentState != null)
                {
                    _interface.Draw(Main.spriteBatch, _lastUpdateUI);
                }
                return true;
            }, InterfaceScaleType.UI));
        }
    }
}
