using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class RocketWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.CloneDefaults(ProjectileID.RocketI);
            AIType = ProjectileID.RocketI;
            Projectile.damage = 1;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                var velocity = new Vector2(10, 0).RotatedByRandom(MathHelper.ToRadians(180));
                Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Projectile.Center, velocity, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            Projectile.timeLeft = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            this.Kill(0);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            base.AI();
        }
    }
}
