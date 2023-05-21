
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode.MechanicalWaterProjectiles
{
    public class SpearProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 102;
            Projectile.height = 48;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 210;
        }

        public Vector2 dist;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            dist = -(Main.player[Main.myPlayer].Center - Projectile.Center);
        }

        int delay = 0;
        int dir = 1;
        float degreeOffset = 0;
        public override void AI()
        {
            base.AI();

            var relativePos = Main.player[Main.myPlayer].Center - dist;
            var degree = relativePos.DirectionTo(Main.MouseWorld).ToRotation();

            var startPos = Main.MouseWorld - (new Vector2(Projectile.width * 0.9f - delay, 0).RotatedBy(degree + degreeOffset));


            Projectile.Center = startPos;

            if (delay > Projectile.width / 2)
            {
                dir = -2;
                degreeOffset = Main.rand.NextFloat(-0.4f, 0.4f);
            }
            else if (delay < 0)
            {
                dir = 1;
            }

            delay += dir * 6;


            Projectile.spriteDirection = (Main.MouseWorld.X - Projectile.Center.X > 0) ? 1 : -1;
            Projectile.rotation = Projectile.Center.AngleTo(Main.MouseWorld) - (Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi);



        }
    }
}
