using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

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
            if (Main.player[Projectile.owner].HeldItem.damage < 88)
            {
                Main.player[Projectile.owner].HeldItem.damage += 2;
                if (Main.player[Projectile.owner].HeldItem.damage >= 88)
                {
                    SoundEngine.PlaySound(SoundID.Item4);
                }
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(new Color(255, 255, 255), 1f);
        }
    }
}
