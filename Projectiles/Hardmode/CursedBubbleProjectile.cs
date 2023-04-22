using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public class CursedBubbleProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 90;

            Projectile.penetrate = 1;
            Projectile.tileCollide = true;

            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 75;

            base.affectedByAmmoBuff = false;
        }


        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Projectile.scale *= Main.rand.NextFloat(0.6f, 1f) + 1f;
            Projectile.velocity *= Main.rand.NextFloat(0.8f, 1.2f);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = Projectile.Center;
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), 75, new Color(179, 252, 0), 0.8f);
                dust.noGravity = true;
                dust.velocity = speed * 6;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }


        public override void AI()
        {
            base.AI();

            Projectile.velocity *= 0.984f;
        }
    }
}
