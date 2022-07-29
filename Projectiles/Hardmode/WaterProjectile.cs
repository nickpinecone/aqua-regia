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

        int direction = 1;

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

                if (data.mysterious != 0)
                {
                    direction = data.mysterious;
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(45 * direction));
                    direction = -direction;
                }
            }
            base.OnSpawn(source);
        }

        int delay = 0;
        int delayMax = 5;
        public override void AI()
        {
            if (base.data.mysterious != 0 && base.data.homesIn)
            {
                Projectile.penetrate = 1;
            }

            base.AI();
            if (data.mysterious != 0)
            {
                delay += 1;
                if (delay > delayMax)
                {
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90 * direction));
                    direction = -direction;

                    delay = 0;
                    delayMax = 10;
                }
            }
            base.CreateDust(color, dustScale, dustAmount, fadeIn, alpha);
        }
    }
}
