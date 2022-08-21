using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.NPCs.Gores
{
    public class Swimmer_Gore2 : ModGore
    {
        public override void OnSpawn(Terraria.Gore gore, IEntitySource source)
        {
            gore.timeLeft = Gore.goreTime * 3;
            base.OnSpawn(gore, source);
        }
    }
}
