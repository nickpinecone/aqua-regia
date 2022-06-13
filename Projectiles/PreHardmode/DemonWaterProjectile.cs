using Microsoft.Xna.Framework;
using Terraria;
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
            int direction = Main.MouseWorld.X - Projectile.position.X > 0 ? 1 : -1;

            var modifiedVelocity = new Vector2(1 * direction, 0).RotatedBy(MathHelper.ToRadians(225 * -direction)).RotatedByRandom(MathHelper.ToRadians(5));
            modifiedVelocity *= 10;
            var offset = new Vector2(Projectile.position.X + 128 * direction, Projectile.position.Y - 128);

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset, modifiedVelocity, Projectile.type, Projectile.damage, Projectile.knockBack);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
