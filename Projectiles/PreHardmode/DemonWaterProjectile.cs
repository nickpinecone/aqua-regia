using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class SwordSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.timeLeft = 40;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.extraUpdates = 0;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 75;
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }

        Vector2 velocity;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = MathHelper.PiOver4;
            delayRandomness = Main.rand.Next(-2, 2);
            Projectile.ai[0] = 15f;
            Projectile.alpha = 255;

            velocity = Projectile.velocity * 2f;
            Projectile.velocity = Vector2.Zero;

            base.OnSpawn(source);
        }

        int delay = 10;
        int delayRandomness = 0;
        bool pastTarget = false;
        int direction = 0;
        float rotateAmount = 0;

        public override void AI()
        {
            if (Projectile.ai[0] > 0f)
            {
                Projectile.ai[0] -= 1;
                Projectile.alpha -= 255 / 15;
            }
            else
            {
                Projectile.velocity = velocity;
            }

            if (Projectile.timeLeft <= 10)
            {
                Projectile.alpha += 255 / 10;
            }
        }

    }

    public class DemonWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int rotation = Main.rand.Next(-90, 0);
            var randomPosition = target.Center + new Vector2(256, 0).RotatedBy(MathHelper.ToRadians(-45)).RotatedBy(MathHelper.ToRadians(rotation));
            var modifiedVelocity = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(rotation - 180 - 45));

            // Spawn default water projectile
            Projectile.NewProjectile(base.data, randomPosition, modifiedVelocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}
