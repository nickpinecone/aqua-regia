using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.UI;
using System.Collections.Generic;
using Terraria;

namespace WaterGuns
{
    public class ModUtils : ModSystem
    {
        internal UI.Canvas Canvas;
        private UserInterface _canvas;

        public override void Load()
        {
            Canvas = new UI.Canvas();
            Canvas.Activate();
            _canvas = new UserInterface();
            _canvas.SetState(Canvas);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _canvas?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "YourMod: A Description",
                    delegate
                    {
                        _canvas.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void AddRecipeGroups()
        {
            Terraria.RecipeGroup goldBars = new Terraria.RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            Terraria.RecipeGroup.RegisterGroup("MoreWaterGuns:GoldBars", goldBars);

            Terraria.RecipeGroup titaniumBars = new Terraria.RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Titanium Bar", new int[]
            {
                ItemID.TitaniumBar,
                ItemID.AdamantiteBar
            });
            Terraria.RecipeGroup.RegisterGroup("MoreWaterGuns:TitaniumBars", titaniumBars);
            base.AddRecipeGroups();
        }
    }
}
