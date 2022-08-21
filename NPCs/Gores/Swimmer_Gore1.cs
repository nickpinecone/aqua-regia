using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.NPCs.Gores
{
    public class Swimmer_Gore1 : ModGore
    {
        public override string Texture => "WaterGuns/NPCs/Gores/Swimmer_Gore1";

        public override void OnSpawn(Terraria.Gore gore, IEntitySource source)
        {
            gore.numFrames = 15;
            gore.behindTiles = true;
            gore.timeLeft = Gore.goreTime * 3;
            base.OnSpawn(gore, source);
        }
    }
}
