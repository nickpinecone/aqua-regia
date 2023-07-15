using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class IceWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.IceBolt);
            AIType = ProjectileID.IceBolt;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.scale *= 1.2f;

            base.defaultDust = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 240);
            base.OnHitNPC(target, hit, damageDone);
        }

        Vector2 spin = new Vector2(30, 0);
        public override void AI()
        {
            if (Projectile.velocity != Vector2.Zero)
            {
                base.AI();
            }
            else
            {
                UpdateImmunityFrames();
                spin = spin.RotatedBy(MathHelper.ToRadians(2.2f));
                Projectile.position = Main.player[Main.myPlayer].position + spin;
            }
        }
    }
}
