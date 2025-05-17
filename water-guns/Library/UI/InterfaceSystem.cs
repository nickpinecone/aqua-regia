using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace WaterGuns.Library.UI;

public interface IProgressState
{
    public HStack BoxContainer { get; set; }
    public FillBox PrimaryBox { get; set; }
}

public abstract class InterfaceSystem : ModSystem
{
    private UserInterface? _interface;
    private GameTime? _lastUpdateUI;
    private UIState? _state;

    private string _afterLayer = "";
    private string _layerName = "";

    protected abstract void Initialize(out UIState state, out string afterLayer, out string layerName);

    public override void Load()
    {
        if (Main.dedServ) return;
        
        base.Load();
        Initialize(out _state, out _afterLayer, out _layerName);

        _interface = new UserInterface();
        _state.Activate();
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

        var layerIndex = layers.FindIndex(layer => layer.Name.Equals(_afterLayer));

        if (layerIndex == -1) throw new Exception($"Could not find layer: {_afterLayer}");

        layers.Insert(layerIndex, new LegacyGameInterfaceLayer(_layerName, delegate
        {
            if (_lastUpdateUI != null && _interface?.CurrentState != null)
            {
                _interface.Draw(Main.spriteBatch, _lastUpdateUI);
            }

            return true;
        }, InterfaceScaleType.UI));
    }
}