using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace WaterGuns
{
    public class WaterGuns : Mod
    {
        public class ProjectileData : IEntitySource
        {
            public string context = "";
            public string Context { get { return context; } }
            public ProjectileData(IEntitySource original)
            {
                context = original.Context;
            }

            public Color color = default;
            public int dustAmount = 4;
            public float dustScale = 1.2f;
            public float fadeIn = 1;
            public int alpha = 75;

            // Effects
            public bool hasBuff = false;
            public int buffType = 0;
            public int buffTime = 0;

            public bool bounces = false;
            public bool homesIn = false;

            // Soundwave
            public bool isUp = true;

            // Mysterious mode
            public int mysterious = 0;
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
