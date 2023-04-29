using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Buffs
{
    public class TurretSummonBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Description.SetDefault("Turret Sentury");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 60;
        }
    }
}
