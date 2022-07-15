using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class DemonWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            // Projectile.CloneDefaults(ProjectileID.WaterGun);
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int rotation = Main.rand.Next(-90, 0);
            var randomPosition = target.Center + new Vector2(256, 0).RotatedBy(MathHelper.ToRadians(-45)).RotatedBy(MathHelper.ToRadians(rotation));
            var modifiedVelocity = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(rotation - 180 - 45));

            // Spawn default water projectile
            Projectile.NewProjectile(base.data, randomPosition, modifiedVelocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
