using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class IchorWaterProjectile : BaseProjectile
    {
        public bool second = false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.tileCollide = false;
            Projectile.timeLeft += 20;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            base.AI();

            base.CreateDust(new Color(255, 250, 41), 1.2f);

            // Ichor dust that emits little light
            var dust2 = Dust.NewDust(Projectile.position, 5, 5, DustID.Ichor, 0, 0, 0, default, 0.8f);
            Main.dust[dust2].noGravity = true;
        }
    }
}
