using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class LavaWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            base.AI();

            // Creating some dust to see the projectile
            var offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            offset.Normalize();
            offset *= 3;

            for (int i = 0; i < 4; i++)
            {
                var position = new Vector2(Projectile.Center.X + offset.X * i, Projectile.Center.Y + offset.Y * i);
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), 0, new Color(255, 215, 70));
                dust.noGravity = true;
                dust.fadeIn = 1;
            }

            // Fire dust that emits light
            var dust2 = Dust.NewDust(Projectile.position, 5, 5, DustID.Flare, 0, 0, 0, default, 3f);
            Main.dust[dust2].noGravity = true;
        }
    }
}
