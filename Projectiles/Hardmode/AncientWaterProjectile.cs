using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class AncientGeyser : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 64;
            Projectile.height = 448;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
        }
    }

    public class AncientWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Item14);

            base.OnHitNPC(target, damage, knockback, crit);
            target.AddBuff(BuffID.OnFire3, 60 * 2);

            var position = new Vector2();

            for (int k = 0; k < Main.ViewSize.Y; k += 16)
            {
                var tilePosition = (target.Center + new Vector2(0, k)).ToTileCoordinates();
                if (Main.tile[tilePosition.X, tilePosition.Y].HasTile && Main.tile[tilePosition.X, tilePosition.Y].BlockType == BlockType.Solid)
                {
                    position = tilePosition.ToWorldCoordinates();
                    break;
                }
            }

            var proj = Projectile.NewProjectileDirect(data, position, new Vector2(0, 0), ModContent.ProjectileType<AncientGeyser>(), Projectile.damage * 2, 0, Projectile.owner);
            proj.Bottom = position;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    var positionLocal = position + new Vector2((i - 2) * -14, 0);
                    var speed = new Vector2(0, -1).RotatedByRandom(MathHelper.ToRadians(3));

                    var dust = Dust.NewDustPerfect(positionLocal, DustID.Wet, new Vector2(0, 0), 75, default, 4f);
                    dust.noGravity = true;
                    dust.fadeIn = 1.5f;
                    dust.velocity = speed * Main.rand.Next(0, 40);
                }
            }

            for (int i = 0; i < 20; i++)
            {
                var speed = (new Vector2(0, -1)).RotatedByRandom(MathHelper.ToRadians(3));
                var positionLocal = position + new Vector2(Main.rand.Next(-32, 32), 0);

                var dust = Dust.NewDustPerfect(positionLocal, DustID.Smoke, new Vector2(0, 0), 0, default, 3f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
                dust.velocity = speed * Main.rand.Next(0, 40);
            }


            for (int i = 0; i < 20; i++)
            {
                var speed = (new Vector2(0, -1)).RotatedByRandom(MathHelper.ToRadians(3));
                var positionLocal = position + new Vector2(Main.rand.Next(-32, 32), 0);

                var dust = Dust.NewDustPerfect(positionLocal, DustID.Flare, new Vector2(0, 0), 0, default, 3f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
                dust.velocity = speed * Main.rand.Next(0, 40);
            }

        }

        public override void AI()
        {
            base.AI();
            base.CreateDust();
        }
    }
}
