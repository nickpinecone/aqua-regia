using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class LavaWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            // Projectile.CloneDefaults(ProjectileID.WaterGun);
            AIType = ProjectileID.WaterGun;

            Projectile.damage = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 62;

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.extraUpdates = 2;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        float gravity = 0.001f;
        public override void AI()
        {
            // Curve it like the in-game water gun projectile
            gravity += 0.002f;
            Projectile.velocity.Y += gravity;

            // Creating some dust to see the projectile
            var dust = Dust.NewDustPerfect(Projectile.Center, 211, new Vector2(0, 0), 0, new Color(244, 42, 10), 1.5f);
            dust.fadeIn = 1;
            dust.noGravity = true;

            base.AI();
        }
    }
}
