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

            // Full charge
            public bool fullCharge = false;

            // Mysterious mode
            public int mysterious = 0;
        }
    }
}
