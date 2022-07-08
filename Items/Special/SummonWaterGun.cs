using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Special
{
    public class SummonWaterGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ImpStaff);
            Item.shoot = ModContent.ProjectileType<Projectiles.Special.WaterGunSummon>();
        }
    }
}
