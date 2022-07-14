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
        public WaterGuns.ProjectileData data = null;
        public bool defaultDust = true;

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
            if (source is WaterGuns.ProjectileData newData)
            {
                data = newData;
            }
            else
            {
                data = new WaterGuns.ProjectileData(source);
            }
            base.OnSpawn(source);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (data.confusionBuff)
            {
                target.AddBuff(BuffID.Confused, 2);
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public void CreateDust(Color color = default, float scale = 1.2f, int amount = 4, float fadeIn = 1, int alpha = 75)
        {
            if (data.color != default)
            {
                color = data.color;
            }

            // Dust creation resembling the in-game water gun projectile
            var offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            offset.Normalize();
            offset *= 3;

            for (int i = 0; i < data.dustAmount; i++)
            {
                var position = new Vector2(Projectile.Center.X + offset.X * i, Projectile.Center.Y + offset.Y * i);
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), alpha, color, scale);
                dust.noGravity = true;
                dust.fadeIn = fadeIn;
            }
        }

        protected float gravity = 0.001f;
        public override void AI()
        {
            // Curve it like the in-game water gun projectile
            gravity += 0.002f;
            Projectile.velocity.Y += gravity;

            // The dust should be created in the child class
            if (defaultDust)
                CreateDust(data.color, data.dustScale, data.dustAmount, data.fadeIn, data.alpha);

            base.AI();
        }
    }
}
