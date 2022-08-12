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
            panel = new UIPanel(); // 2
            panel.Width.Set(10, 0); // 3
            panel.Height.Set(10, 0); // 3
            position = new Vector2(Main.screenWidth + 10, Main.screenHeight - 20) / 2f;
            panel.Top.Set(position.Y, 0);
            panel.Left.Set(position.X, 0);
            Append(panel); // 4
        }

        public override void RightClick(UIMouseEvent evt)
        {
            if (Main.player[Main.myPlayer].HeldItem.ModItem is Items.CommonWaterGun gun)
            {
                panel.Height.Set(10 + (5 * (gun.pumpLevel + 1)), 0);
                panel.Top.Set(position.Y - 5 * (gun.pumpLevel + 1), 0);
            }
            base.RightClick(evt);
        }

        public override void Click(UIMouseEvent evt)
        {
            if (Main.player[Main.myPlayer].HeldItem.ModItem is Items.CommonWaterGun gun)
            {
                panel.Height.Set(10 + (5 * (gun.pumpLevel + 1)), 0);
                panel.Top.Set(position.Y - 5 * (gun.pumpLevel + 1), 0);
            }
            base.Click(evt);
        }
    }
}
