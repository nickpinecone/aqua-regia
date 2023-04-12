using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace WaterGuns.Ammo
{
    public abstract class BaseAmmo : ModItem
    {
        public int damage = 0;
        public bool hasBuff = false;
        public int buffType = 0;
        public int buffTime = 0;
        public Color color = new Color();

        public bool homesIn = false;
        public bool bounces = false;
        public bool spawnsStar = false;
        public bool penetrates = false;

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BottledWater);
            Item.potion = false;
            Item.healLife = 0;
            Item.ammo = ItemID.BottledWater;

        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.Confused, 240);
            return true;
        }
    }
}
