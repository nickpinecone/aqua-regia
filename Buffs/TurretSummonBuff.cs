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
        }
    }
}
