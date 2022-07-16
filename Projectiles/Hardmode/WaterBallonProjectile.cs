using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class WaterExplosion : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.damage = 1;
            Projectile.scale *= 4;
            Projectile.timeLeft = 12;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
    }

    public class WaterBallonProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.Grenade;
            Projectile.timeLeft = 600;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item85);

            Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<WaterExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            for (int i = 0; i < 14; i++)
            {
                var rotation = Main.rand.Next(0, 360);
                var velocity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(rotation));
                velocity *= 2f;
                var dust = Dust.NewDustDirect(Projectile.position, 30, 30, DustID.Wet, velocity.X, velocity.Y, 75, default, 1.4f);
            }

            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;

            return false;
        }

        protected float gravity = 0.04f;
        public override void AI()
        {
            Projectile.velocity.Y += gravity;
            Projectile.rotation += MathHelper.ToRadians(10);
            Projectile.velocity *= 0.998f;

            base.AI();
        }
    }
}
