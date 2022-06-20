using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class AcornProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            AIType = ProjectileID.WoodenArrowFriendly;

            Projectile.damage = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;

            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }
    }

    public class WoodenWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WaterGun);
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.player[Main.myPlayer].IsTileTypeInInteractionRange(TileID.Trees))
            {
                var position = target.position;
                position.Y -= 320;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, new Vector2(0, 10), ModContent.ProjectileType<AcornProjectile>(), 4, Projectile.knockBack, Projectile.owner);
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
