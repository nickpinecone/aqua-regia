using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;

namespace WaterGuns.Projectiles.PreHardmode
{
    public abstract class BaseProjectile : ModProjectile
    {
        public Color color;
        public int dustAmount;
        public float dustScale;
        public float fadeIn;

        public override void SetDefaults()
        {
            // If derivatives dont call base.SetDefaults() they use Projectile.CloneDefaults(ProjectileID.WaterGun);
            // These are for creating custom projectiles resembling water gun projectile
            AIType = ProjectileID.WaterGun;

            Projectile.damage = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 62;

            Projectile.width = 16;
            Projectile.height = 16;

            // Whithout extra updates it feels slow
            Projectile.extraUpdates = 2;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (source is WaterGuns.ProjectileData data)
            {
                color = data.color;
                dustAmount = data.dustAmount;
                dustScale = data.dustScale;
                fadeIn = data.fadeIn;
            }
            base.OnSpawn(source);
        }

        public void CreateKillEffect(Color color = default, float scale = 0.7f)
        {
            for (int i = 0; i < 14; i++)
            {
                var offset = new Vector2(Projectile.Center.X - MathF.Abs(Projectile.velocity.X * 48), Projectile.Center.Y - MathF.Abs(Projectile.velocity.Y * 48));

                Projectile.velocity.Normalize();
                var velocity = (Projectile.velocity * 4).RotatedByRandom(MathHelper.ToRadians(10));

                var dust = Dust.NewDust(offset, 50, 5, DustID.Wet, velocity.X, velocity.Y, 0, color, scale);
            }
        }

        public void CreateDust(Color color, float scale)
        {
            // Dust creation resembling the in-game water gun projectile
            var offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            offset.Normalize();
            offset *= 3;

            for (int i = 0; i < 4; i++)
            {
                var position = new Vector2(Projectile.Center.X + offset.X * i, Projectile.Center.Y + offset.Y * i);
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), 0, color, scale);
                dust.noGravity = true;
                dust.fadeIn = 1;
            }
        }

        protected float gravity = 0.001f;
        public override void AI()
        {
            // Curve it like the in-game water gun projectile
            gravity += 0.002f;
            Projectile.velocity.Y += gravity;

            // The dust should be created in the child class
            // base.CreateDust(...)

            base.AI();
        }
    }
}
