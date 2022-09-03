using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WaterGuns.UI
{
    class Canvas : UIState
    {
        public UIPanel panel;
        Vector2 position;

        public override void OnInitialize()
        {
            panel = new UIPanel();
            panel.Width.Set(10, 0);
            panel.Height.Set(10, 0);
            position = new Vector2(Main.screenWidth, Main.screenHeight - 20) / 2f;
            panel.Top.Set(position.Y, 0);
            panel.Left.Set(position.X, 0);
            Append(panel);
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.player[Main.myPlayer].HeldItem.ModItem is Items.CommonWaterGun gun)
            {
                panel.BorderColor = new Color(255, 255, 255, 0);
                panel.BackgroundColor = new Color(255, 255, 255, 0);

                panel.Height.Set(10 + (5 * (gun.pumpLevel)), 0);
                panel.Top.Set(position.Y - 5 * (gun.pumpLevel), 0);
            }
            else
            {
                panel.BorderColor = Color.Transparent;
                panel.BackgroundColor = Color.Transparent;
            }
            base.Update(gameTime);
        }
    }
}
