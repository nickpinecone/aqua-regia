using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class WaterBallonGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots ballons filled with water");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 36;
            Item.knockBack = 3;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.WaterBallonProjectile>();
            Item.shootSpeed -= 7;
        }
    }
}

