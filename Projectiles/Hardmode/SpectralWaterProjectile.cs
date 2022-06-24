using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class SpectralWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.player[Projectile.owner].HeldItem.damage += 1;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(new Color(255, 255, 255), 1f);
        }
    }
}
