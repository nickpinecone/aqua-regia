using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class LavaWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultDust = false;
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
            base.CreateDust(new Color(255, 215, 50));

            // Fire dust that emits light
            var dust2 = Dust.NewDust(Projectile.position, 5, 5, DustID.Flare, 0, 0, 0, default, 3f);
            Main.dust[dust2].noGravity = true;
        }
    }
}
