using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class WaterLaser : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 1;
            Projectile.timeLeft = 74;

            Projectile.width = 16;
            Projectile.height = 330;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CreateDustLaser();

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item12);
        }

        List<Dust> dusts = new List<Dust>();
        public void CreateDustLaser(Color color = default, float scale = 1.2f, int alpha = 75)
        {
            for (int i = 0; i < 100; i++)
            {
                var position = new Vector2(Projectile.Top.X, Projectile.Top.Y + (330 / 100f) * i);
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), alpha, color, scale);
                dust.noGravity = true;
                dust.fadeIn = 2;
                dust.scale = 4;

                dusts.Add(dust);
            }
        }

        int delay = 0;
        public override void AI()
        {
            Projectile.position += new Vector2(-5, 0);
            for (int i = 0; i < dusts.Count; i++)
            {
                dusts[i].position += new Vector2(-5, 0);
            }


            var rotation = Main.rand.Next(-75, 75);
            var velocity = new Vector2(0, -1.4f).RotatedBy(MathHelper.ToRadians(rotation));
            velocity *= 8f;
            var dust = Dust.NewDust(Projectile.Bottom + new Vector2(-8, -10), 24, 24, DustID.Wet, velocity.X, velocity.Y, 75, default, 1.4f);
            Main.dust[dust].noGravity = true;
        }
    }


    public class SpaceWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.penetrate = 2;
        }


        int counter = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (counter < 4)
            {
                counter += 1;

                // // Bounce off the wall without creating a new projectile
                // var velocity = -oldVelocity.RotatedByRandom(MathHelper.ToRadians(45));
                // Projectile.velocity = velocity;


                if (oldVelocity.X != Projectile.velocity.X) Projectile.velocity.X = -oldVelocity.X;
                if (oldVelocity.Y != Projectile.velocity.Y) Projectile.velocity.Y = -oldVelocity.Y;

                // Reset gravity and timeLeft so it doesnt destroy
                Projectile.timeLeft = 62;
                base.gravity = 0.001f;
                return false;
            }
            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (data.fullCharge)
            {
                var offset = target.Bottom + new Vector2(200, -160);
                Projectile.NewProjectile(data, offset, new Vector2(0, 0), ModContent.ProjectileType<WaterLaser>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
