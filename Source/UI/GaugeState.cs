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
    GaugeElement _gauge;
    Asset<Texture2D> _texture;

    public override void OnInitialize()
    {
        _texture = ModContent.Request<Texture2D>(TexturesPath.UI + "GaugeFrame");

        _gauge = new GaugeElement(_texture);
        _gauge.HAlign = 0.99f;
        _gauge.VAlign = 0.98f;

        Append(_gauge);
    }

    public Asset<Texture2D> GetTexture()
    {
        return _texture;
    }

    public void AddGauge(GaugeElement gauge)
    {
        gauge.HAlign = 0.96f;
        gauge.VAlign = 0.98f;

        Append(gauge);
    }

    public void RemoveGauge(GaugeElement gauge)
    {
        RemoveChild(gauge);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!(Main.LocalPlayer.HeldItem.ModItem is BaseGun baseGun && baseGun.HasModule<PumpModule>()))
        {
            _gauge.Hidden = true;
        }
        else
        {
            _gauge.Hidden = false;
            var pump = ((BaseGun)Main.LocalPlayer.HeldItem.ModItem).GetModule<PumpModule>();

            _gauge.Current = pump.PumpLevel;
            _gauge.Max = pump.MaxPumpLevel;
        }

        base.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
