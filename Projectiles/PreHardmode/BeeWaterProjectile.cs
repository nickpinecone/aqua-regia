using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class BeeSwarm : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 80;
            Projectile.friendly = false;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.extraUpdates = 0;
        }

        int delay = 0;
        public override void AI()
        {
            delay += 1;
            if (delay > 14)
            {
                delay = 0;
                var direction = Main.rand.NextBool() ? -1 : 1;
                var modifiedVelocity = new Vector2(10 * -direction, 0);
                var position = Projectile.Center + new Vector2(180 * direction, Main.rand.Next(-32, 32));
                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), position, modifiedVelocity, ProjectileID.GiantBee, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }

    public class BeeWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (data.fullCharge)
            {
                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), target.Center, Vector2.Zero, ModContent.ProjectileType<BeeSwarm>(), 12, Projectile.knockBack, Projectile.owner);
            }
            target.AddBuff(ModContent.BuffType<Buffs.HoneySlowDebuff>(), 60 * 2);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
