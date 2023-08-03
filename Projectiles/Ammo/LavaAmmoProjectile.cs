using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Ammo
{
    public class LavaAmmoProjectile : Projectiles.CommonWaterProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 45;
            base.affectedByAmmoBuff = false;
            Projectile.extraUpdates = 0;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 240);
            base.OnHitNPC(target, hit, damageDone);
        }

        float gravity = 0.04f;
        public override void AI()
        {
            base.AI();
            gravity += 0.02f;
            Projectile.velocity.Y += gravity;

            // Cursed Flame dust that emits light
            var dust2 = Dust.NewDust(Projectile.position, 5, 5, DustID.Flare, 0, 0, 0, default, 1.4f);
            Main.dust[dust2].noGravity = true;
        }
    }
}
