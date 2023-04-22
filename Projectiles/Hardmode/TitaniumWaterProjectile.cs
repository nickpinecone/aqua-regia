using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class DamageZone : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 128;
            Projectile.height = 128;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
        }
    }

    public class TitaniumWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);

            Projectile.NewProjectile(data, target.Center, new Vector2(0, 0), ModContent.ProjectileType<DamageZone>(), Projectile.damage / 2, 0, Projectile.owner);

            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = target.Center;
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), 75, default, 4f);
                dust.noGravity = true;
                dust.velocity = speed * 7;
            }
        }


        public override void AI()
        {
            base.CreateDust();
            base.AI();
        }
    }
}
