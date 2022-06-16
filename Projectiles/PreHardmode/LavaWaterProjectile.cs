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
            var dust = Dust.NewDustPerfect(Projectile.Center, 211, new Vector2(0, 0), 0, new Color(246, 103, 8), 1.5f);
            dust.fadeIn = 1;
            dust.noGravity = true;

            var dust2 = Dust.NewDust(Projectile.position, 10, 10, DustID.Flare);
            Main.dust[dust2].noGravity = true;
        }
    }
}
