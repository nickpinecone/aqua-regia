using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class StrengthSoul : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
        }

        float speed = 6;
        public override void AI()
        {
            base.AI();
            var dust = Dust.NewDustPerfect(Projectile.Center, 76, Vector2.Zero, 30, default, 1f);
            dust.noGravity = true;

            var player = Main.player[Main.myPlayer];
            var velocity = Projectile.Center.DirectionTo(player.Center);
            velocity.Normalize();

            Projectile.velocity = velocity * speed;
            speed += 0.1f;

            if (Projectile.Center.Distance(player.Center) < 16f)
            {
                Projectile.Kill();
                if (player.HeldItem.ModItem is Items.Hardmode.SpectralWaterGun gun)
                {
                    if (gun.Item.damage < (gun.damage + gun.damage / 2))
                    {
                        gun.Item.damage += 1;
                    }
                    gun.delayMax = Items.Hardmode.SpectralWaterGun.DELAY_MAX;
                    gun.delay = 0;
                }
            }
        }
    }

    public class SoulProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.LostSoulFriendly);
            AIType = ProjectileID.LostSoulFriendly;
            Projectile.friendly = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);

            if (target.GetLifePercent() < 0f)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), target.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.StrengthSoul>(), 0, 0, Projectile.owner);
            }
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

            if (target.GetLifePercent() < 0f)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), target.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.StrengthSoul>(), 0, 0, Projectile.owner);
            }
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, 1f);
        }
    }
}
