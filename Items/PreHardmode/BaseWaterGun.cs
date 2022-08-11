using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Items.PreHardmode
{
    public abstract class BaseWaterGun : CommonWaterGun
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultInaccuracy = 3.3f;

            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>();
        }
    }
}
