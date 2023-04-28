using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace WaterGuns.Buffs
{
    public class HoneySlowDebuff : ModBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            base.Update(npc, ref buffIndex);

            if (!npc.boss)
            {
                npc.velocity -= npc.velocity / 12;
            }
        }
    }
}
