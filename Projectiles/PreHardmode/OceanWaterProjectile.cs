using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class BubbleProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bubble);
            AIType = ProjectileID.Bubble;
        }
    }

    public class OceanWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WaterGun);
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            // Offset randomly
            var offset = new Vector2();
            offset.X = Projectile.position.X + Main.rand.Next(-60, 60);
            offset.Y = Projectile.position.Y + Main.rand.Next(0, 60);

            // Dont know how to extract IEventSource from BubbleProjectile so using OceanWaterProjectile source
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset, new Vector2(0, -4), ModContent.ProjectileType<BubbleProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
