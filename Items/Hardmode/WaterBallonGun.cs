using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            DisplayName.SetDefault("Water Ballon Bomber");
            Tooltip.SetDefault("Shoots ballons filled with water");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 36;
            Item.knockBack = 3;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.WaterBallonProjectile>();
            Item.shootSpeed -= 6;
            Item.value = Item.buyPrice(0, 20, 50, 0);

            base.isOffset = true;
            base.offsetAmount = new Vector2(8, 8);
            base.offsetIndependent = new Vector2(0, -6);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}

