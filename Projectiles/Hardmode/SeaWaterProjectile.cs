using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class SeaWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.timeLeft += 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Debuffs.BubbleSuffocateDebuff>(), 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, 1);
        }
    }
}
