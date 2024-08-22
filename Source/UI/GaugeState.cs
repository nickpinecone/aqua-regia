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

    public override void OnInitialize()
    {
        GaugeElement.Texture = ModContent.Request<Texture2D>(TexturesPath.UI + "GaugeFrame");

        _gauge = new GaugeElement();
        _gauge.HAlign = 0.99f;
        _gauge.VAlign = 0.98f;

        Append(_gauge);
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
