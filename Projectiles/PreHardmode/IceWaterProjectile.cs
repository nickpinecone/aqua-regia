using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class FrostWave : ModProjectile
    {
        public override void SetDefaults()
        {
            AIType = ProjectileID.FrostWave;

            Projectile.damage = 1;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;

            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity *= 1.4f;
            if (Projectile.knockBack == 3)
            {
                direction = -direction;
            }
            base.OnSpawn(source);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        int delay = 0;
        int delayMax = 5;
        int direction = 1;
        public override void AI()
        {
            base.AI();

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(4 * direction));

            delay += 1;
            if (delay > delayMax)
            {
                direction = -direction;
                delay = 0;
                delayMax = 11;
            }
        }
    }

    public class IceWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.IceBolt);
            AIType = ProjectileID.IceBolt;
            Projectile.friendly = false;
            Projectile.scale *= 1.2f;

            base.defaultDust = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        Vector2 spin = new Vector2(28, 0);
        public override void AI()
        {
            if (Projectile.velocity != Vector2.Zero)
            {
                base.AI();
            }
            else
            {
                spin = spin.RotatedBy(MathHelper.ToRadians(2f));
                Projectile.position = Main.player[Main.myPlayer].position + spin;
            }
        }
    }
}
