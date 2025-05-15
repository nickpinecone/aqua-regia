using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace WaterGuns.Library.UI;

public abstract class InterfaceSystem : ModSystem
{
    private UserInterface? _interface;
    private GameTime? _lastUpdateUI;
    protected UIState? _state;

    protected string _afterLayer;
    protected string _layerName;

    protected abstract void Initialize();

    public override void Load()
    {
        if (Main.dedServ) return;
        
        base.Load();
        Initialize();

        _interface = new UserInterface();
        _state!.Activate();
        _interface.SetState(_state);
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

        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals(_afterLayer));

        if (mouseTextIndex == -1) throw new Exception($"Could not find layer: {_afterLayer}");

        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(_layerName, delegate
        {
            if (_lastUpdateUI != null && _interface?.CurrentState != null)
            {
                _interface.Draw(Main.spriteBatch, _lastUpdateUI);
            }

            return true;
        }, InterfaceScaleType.UI));
    }
}