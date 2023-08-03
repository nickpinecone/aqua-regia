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
            Append(panel);
        }

        public override void Update(GameTime gameTime)
        {

            if (Main.player[Main.myPlayer].HeldItem.ModItem is Items.CommonWaterGun gun)
            {
                position = new Vector2(Main.screenWidth - 60, Main.screenHeight - 60);
                panel.Top.Set(position.Y, 0);
                panel.Left.Set(position.X, 0);

                panel.BorderColor = new Color(255, 255, 255, 0);
                panel.BackgroundColor = new Color(255, 255, 255, 0);
                panel.SetPadding(1f);

                panel.Height.Set(10 + (30 * ((float)gun.pumpLevel / (float)gun.maxPumpLevel)), 0);
                panel.Top.Set(position.Y - 30 * ((float)gun.pumpLevel / (float)gun.maxPumpLevel), 0);
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
