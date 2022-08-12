using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;

namespace WaterGuns.UI
{
    class PumpLevelUI : UIElement
    {
        Color color = new Color(50, 255, 153);

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(((Texture2D)ModContent.Request<Texture2D>("Terraria/UI/ButtonPlay")), new Vector2(Main.screenWidth + 20, Main.screenHeight - 20) / 2f, color);
            base.Draw(spriteBatch);
        }
    }
}
