using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class DemonWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WaterGun);
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            // Changing the height in order to avoid recursive calls
            if (Projectile.height == 8)
            {
                int direction = Main.MouseWorld.X - Projectile.position.X > 0 ? 1 : -1;

                var modifiedVelocity = new Vector2(1 * direction, 0).RotatedBy(MathHelper.ToRadians(225 * -direction)).RotatedByRandom(MathHelper.ToRadians(5));
                // Speed it up a bit
                modifiedVelocity *= 10;
                var offset = new Vector2(Projectile.position.X + 196 * direction, Projectile.position.Y - 196);

                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), offset, modifiedVelocity, Projectile.type, Projectile.damage, Projectile.knockBack - 1, Projectile.owner);
                proj.height -= 1;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        float gravity = 0.001f;
        public override void AI()
        {
            // Curve it like the in-game water gun projectile
            gravity += 0.002f;
            Projectile.velocity.Y += gravity;

            base.AI();
        }
    }
}
