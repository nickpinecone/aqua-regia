using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using ReLogic.Content;

namespace WaterGuns.Projectiles.Hardmode
{
    // Example Advanced Flail is a complete adaption of Ball O' Hurt projectile. The code has been rewritten a bit to make it easier to follow. Compare this code against the decompiled Terraria code for an example of adapting vanilla code. A few comments and extra code snippets show features from other vanilla flails as well.
    // Example Advanced Flail shows a plethora of advanced AI and collision topics.
    // See ExampleFlail for a simpler but less customizable flail projectile example.
    public class PlantClinger : BaseProjectile
    {
        private const string ChainTexturePath = "WaterGuns/Projectiles/Hardmode/PlantClingerChain"; // The folder path to the flail chain sprite

        public override void SetStaticDefaults()
        {
            // These lines facilitate the trail drawing
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true; // This ensures that the projectile is synced when other players join the world.
            Projectile.width = 56; // The width of your projectile
            Projectile.height = 46; // The height of your projectile
            Projectile.friendly = true; // Deals damage to enemies
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.usesIDStaticNPCImmunity = true; // Used for hit cooldown changes in the ai hook
            Projectile.timeLeft = 160;
            Projectile.tileCollide = false;

            // Vanilla flails all use aiStyle 15, but the code isn't customizable so an adaption of that aiStyle is used in the AI method
        }

        NPC _target = null;
        Vector2 hitPoint;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(BuffID.Poisoned, 120);

            if (_target == null)
            {
                _target = target;

                hitPoint = Projectile.Center - target.Center;
            }
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            for (int i = 0; i < 6; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = Projectile.Center;
                var dust = Dust.NewDustDirect(position, 8, 8, DustID.JungleGrass, 0, 0, 0, default, 1f);
                dust.noGravity = true;
                dust.velocity = speed * 2;
            }
        }


        public override void AI()
        {
            base.AI();

            if (Projectile.timeLeft <= 3)
            {
                if (Projectile.Center.Distance(Main.player[Main.myPlayer].Center) < 16f)
                {
                    Projectile.timeLeft = 0;
                    Projectile.Kill();
                }
                else
                {
                    var dir = Projectile.Center.DirectionTo(Main.player[Main.myPlayer].Center);

                    Projectile.velocity = dir * 24;
                    Projectile.timeLeft = 3;
                }

                Projectile.rotation = Projectile.velocity.ToRotation();
            }

            else
            {
                if (_target != null)
                {
                    Projectile.Center = _target.Center + hitPoint;

                    if (_target.GetLifePercent() <= 0f)
                    {
                        _target = null;
                    }
                }
                else
                {
                    AutoAim();
                }

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            }


            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }

        // PreDraw is used to draw a chain and trail before the projectile is drawn normally.
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);

            // This fixes a vanilla GetPlayerArmPosition bug causing the chain to draw incorrectly when stepping up slopes. The flail itself still draws incorrectly due to another similar bug. This should be removed once the vanilla bug is fixed.
            playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;

            Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>(ChainTexturePath);

            Rectangle? chainSourceRectangle = null;
            // Drippler Crippler customizes sourceRectangle to cycle through sprite frames: sourceRectangle = asset.Frame(1, 6);
            float chainHeightAdjustment = 0f; // Use this to adjust the chain overlap. 

            Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 4f) : (chainTexture.Size() / 4f);
            Vector2 chainDrawPosition = Projectile.Center;
            Vector2 vectorFromProjectileToPlayerArms = playerArmPosition.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
            Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
            {
                chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
            }
            float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
            int chainCount = 0;
            float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

            if (chainLengthRemainingToDraw > 1400f)
            {
                Projectile.Kill();
            }

            // This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
            while (chainLengthRemainingToDraw > 0f)
            {
                // This code gets the lighting at the current tile coordinates
                Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));

                // Flaming Mace and Drippler Crippler use code here to draw custom sprite frames with custom lighting.
                // Cycling through frames: sourceRectangle = asset.Frame(1, 6, 0, chainCount % 6);
                // This example shows how Flaming Mace works. It checks chainCount and changes chainTexture and draw color at different values

                var chainTextureToDraw = chainTexture;

                // Here, we draw the chain texture at the coordinates
                Main.spriteBatch.Draw(chainTextureToDraw.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

                // chainDrawPosition is advanced along the vector back to the player by the chainSegmentLength
                chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
                chainCount++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }

            return true;
        }
    }

    public class ChlorophyteWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;

            Projectile.timeLeft += 20;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust();
            base.AutoAim();
        }
    }
}
