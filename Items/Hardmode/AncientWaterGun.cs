using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Items.Hardmode
{
    public class AncientWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Geyser");
            Tooltip.SetDefault("Unleashes a geyser on your enemies");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 64;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.AncientWaterProjectile>();

            Item.useTime += 24;
            Item.useAnimation += 24;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, -2);
        }
    }
}
