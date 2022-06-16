using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class DemonWaterProjectile : BaseProjectile
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
            // Speed it up a bit
            modifiedVelocity *= 10;
            var offset = new Vector2(Projectile.position.X + 196 * direction, Projectile.position.Y - 196);

            // Spawn default water projectile
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), offset, modifiedVelocity, ProjectileID.WaterGun, Projectile.damage, Projectile.knockBack, Projectile.owner);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
