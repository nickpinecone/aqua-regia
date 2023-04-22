using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Ammo
{
    public class VenomAmmoProjectile : Projectiles.CommonWaterProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            base.affectedByAmmoBuff = false;
            Projectile.extraUpdates = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            base.AI();

            // Cursed Flame dust that emits light
            var dust2 = Dust.NewDust(Projectile.position, 5, 5, DustID.Venom, 0, 0, 0, default, 1.2f);
            Main.dust[dust2].noGravity = true;
        }
    }
}
