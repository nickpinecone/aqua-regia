using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class SoulProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.LostSoulFriendly);
            AIType = ProjectileID.LostSoulFriendly;
            Projectile.friendly = true;
        }
    }

    public class SpectralWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.timeLeft += 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);

            var dist = new Vector2(Main.rand.Next(12, 24), 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 360)));
            var player = Main.player[Main.myPlayer];

            var projVelocity = -Main.MouseWorld.DirectionTo(player.Center);
            projVelocity.Normalize();
            projVelocity *= 8;
            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.Center + dist, projVelocity, ModContent.ProjectileType<Projectiles.Hardmode.SoulProjectile>(), damage, 2, player.whoAmI);
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, 1f);
        }
    }
}
