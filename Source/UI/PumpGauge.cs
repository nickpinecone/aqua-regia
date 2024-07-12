using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using WaterGuns.Utils;
using WaterGuns.Weapons;
using WaterGuns.Weapons.Modules;

namespace WaterGuns.UI;

class PumpGauge : UIState
{
    private UIImage _frame;

    public override void OnInitialize()
    {
        var texture = ModContent.Request<Texture2D>(TexturesPath.UI + "GaugeFrame");

        _frame = new UIImage(texture);
        _frame.Width.Set(18, 0);
        _frame.Height.Set(90, 0);
        _frame.HAlign = 0.99f;
        _frame.VAlign = 0.98f;

        Append(_frame);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!(Main.LocalPlayer.HeldItem.ModItem is BaseGun baseGun && baseGun.HasModule<PumpModule>()))
        {
            return;
        }

        base.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        if (!(Main.LocalPlayer.HeldItem.ModItem is BaseGun baseGun && baseGun.HasModule<PumpModule>()))
        {
            return;
        }

        base.Update(gameTime);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        this.DisplayTooltip();

        var pump = ((BaseGun)Main.LocalPlayer.HeldItem.ModItem).GetModule<PumpModule>();
        float percent = (float)pump.PumpLevel / (float)pump.MaxPumpLevel;

        if(percent >= 1f)
        {
            _frame.Color = Color.Gold;
        }
        else
        {
            _frame.Color = Color.White;
        }

        Rectangle rectangle = _frame.GetInnerDimensions().ToRectangle();
        rectangle.X += 2;
        rectangle.Width -= 4;
        rectangle.Y += 2;
        rectangle.Height -= 4;

        int steps = (int)((rectangle.Bottom - rectangle.Top) * percent);

        for (int i = 0; i < steps; i += 1)
        {
            float gradient = (float)i / (rectangle.Bottom - rectangle.Top);

            spriteBatch.Draw(TextureAssets.MagicPixel.Value,
                             new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - i, rectangle.Width, 1),
                             Color.Lerp(Color.Blue, Color.Cyan, gradient));
        }
    }

    private void DisplayTooltip()
    {
        if (_frame.IsMouseHovering)
        {
            Main.hoverItemName = "Pump Gauge";
        }
    }
}
