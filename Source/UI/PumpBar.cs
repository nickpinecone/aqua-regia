using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using WaterGuns.Weapons;
using WaterGuns.Weapons.Modules;

namespace WaterGuns.UI;

class PumpBar : UIState
{
    private UIImage _outline;
    private UIPanel _bar;
    private Vector2 _position;

    public override void OnInitialize()
    {
        var outlineTexture = ModContent.Request<Texture2D>("WaterGuns/Assets/Textures/UI/BarOutline");
        _outline = new UIImage(outlineTexture);
        _outline.Top = StyleDimension.FromPercent(0.5f);
        _outline.Left = StyleDimension.FromPercent(0.5f);

        _bar = new UIPanel();
        _bar.Width.Set(20, 0);
        _bar.Height.Set(20, 0);
        _position = new Vector2(2, _outline.Height.Pixels - _bar.Height.Pixels / 2 - 2);
        _bar.Top.Set(_position.Y, 0);
        _bar.Left.Set(_position.X, 0);

        _outline.Append(_bar);

        Append(_outline);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Main.LocalPlayer.HeldItem.ModItem is BaseGun baseGun && baseGun.HasModule<PumpModule>())
        {
            var pump = baseGun.GetModule<PumpModule>();

            _bar.Top.Set(_position.Y, 0);
            _bar.Left.Set(_position.X, 0);

            _bar.BorderColor = Color.White;
            _bar.BackgroundColor = Color.White;

            var value = 100 * ((float)pump.PumpLevel / pump.MaxPumpLevel);
            _bar.Height.Set(value, 0);
            _bar.Top.Set(_position.Y - value, 0);
        }
        else
        {
            _bar.BorderColor = Color.Transparent;
            _bar.BackgroundColor = Color.Transparent;
        }

        _bar.BorderColor = Color.White;
        _bar.BackgroundColor = Color.White;
    }
}
