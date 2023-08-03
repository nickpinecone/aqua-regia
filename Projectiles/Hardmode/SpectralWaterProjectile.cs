using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WaterGuns.Projectiles.Hardmode
{
    public class FriendlyGhost : BaseProjectile
    {
        public override string Texture => "WaterGuns/Projectiles/Hardmode/WaterProjectile";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 4;
            Projectile.usesIDStaticNPCImmunity = true;
        }

        public int ghostId = -1;

        Vector2 baseVelocity;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            baseVelocity = Projectile.velocity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (ghostId != -1)
            {
                // Get texture of projectile
                Asset<Texture2D> texture = ModContent.Request<Texture2D>($"Terraria/Images/NPC_{ghostId}");

                Main.spriteBatch.Draw(texture.Value, Projectile.Center - Main.screenPosition - new Vector2(texture.Value.Bounds.Width / 2, texture.Value.Bounds.Height / Main.npcFrameCount[ghostId] / 2), texture.Frame(1, Main.npcFrameCount[ghostId]), new Color(55, 105, 255, 155), 0, Vector2.Zero, 1f, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }

            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            base.AI();
            AutoAim(600);
            Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
        }
    }

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
                var sound = Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath6);

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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (target.GetLifePercent() < 0f)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), target.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.StrengthSoul>(), 0, 0, Projectile.owner);
                if (Main.player[Main.myPlayer].HeldItem.ModItem is Items.Hardmode.SpectralWaterGun spectralGun)
                {
                    spectralGun.AddEnemy(target);
                }
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (target.GetLifePercent() < 0f)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), target.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.StrengthSoul>(), 0, 0, Projectile.owner);
                if (Main.player[Main.myPlayer].HeldItem.ModItem is Items.Hardmode.SpectralWaterGun spectralGun)
                {
                    spectralGun.AddEnemy(target);
                }
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            data.dustScale = 1;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust();
        }
    }
}