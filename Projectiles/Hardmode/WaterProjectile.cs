using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class WaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public Color color = default;
        public int dustAmount = 4;
        public float dustScale = 1.2f;
        public float fadeIn = 1;
        public int alpha = 75;
        public override void OnSpawn(IEntitySource source)
        {
            if (source is WaterGuns.ProjectileData data)
            {
                color = data.color;
                dustAmount = data.dustAmount;
                dustScale = data.dustScale;
                fadeIn = data.fadeIn;
                alpha = data.alpha;
            }
            base.OnSpawn(source);
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(color, dustScale, dustAmount, fadeIn, alpha);
        }
    }
}
