using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class RocketWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RocketI);
            AIType = ProjectileID.RocketI;
            Projectile.damage = 1;
            Projectile.timeLeft = 120;
            hasKillEffect = false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14);

            for (int i = 0; i < 3; i++)
            {
                var velocity = new Vector2(10, 0).RotatedByRandom(MathHelper.ToRadians(180));
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Projectile.Center, velocity, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                proj.tileCollide = false;
                proj.timeLeft -= 5;
            }
            Projectile.timeLeft = 0;

            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            this.Kill(0);
            return false;
        }

        public override void AI()
        {
            base.AI();
            base.AutoAim();

            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
