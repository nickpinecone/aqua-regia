using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class PullChainProjectile : ModProjectile
    {
        private const string ChainTexturePath = "WaterGuns/Projectiles/PreHardmode/PullChain"; // The folder path to the flail chain sprite

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 160;

            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.hostile = false;
        }

        Vector2 originPosition = Vector2.Zero;
        Vector2 initialVelocity = Vector2.Zero;
        public override void OnSpawn(IEntitySource source)
        {
            originPosition = Projectile.Center;
            initialVelocity = Projectile.velocity;
            base.OnSpawn(source);
        }

        NPC pullTarget = null;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (pullTarget == null)
            {
                pullTarget = target;
                Projectile.velocity = -Projectile.velocity;
                forward = false;
                Projectile.damage = 0;
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        bool forward = true;
        bool hold = false;
        public override void AI()
        {
            var distToOrigin = (Projectile.Center - originPosition).Length();

            if (distToOrigin > 800f && forward)
            {
                Projectile.velocity = -Projectile.velocity;
                forward = false;
            }
            if (distToOrigin < 16f && !forward)
            {
                Projectile.velocity = Vector2.Zero;
            }

            if (pullTarget != null && !hold)
            {
                pullTarget.Center = Projectile.Center;
            }

            base.AI();
        }


        // PreDraw is used to draw a chain and trail before the projectile is drawn normally.
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 playerArmPosition = originPosition;

            // This fixes a vanilla GetPlayerArmPosition bug causing the chain to draw incorrectly when stepping up slopes. The flail itself still draws incorrectly due to another similar bug. This should be removed once the vanilla bug is fixed.
            // playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;

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

    public class WaterGunProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.timeLeft = 160;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public WaterGuns.ProjectileData data = null;
        public override void OnSpawn(IEntitySource source)
        {
            if (source is WaterGuns.ProjectileData newData)
            {
                data = newData;
            }
            else
            {
                data = new WaterGuns.ProjectileData(source);
            }

            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
            base.OnSpawn(source);
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);

            foreach (var chain in shotChains)
            {
                chain.Kill();
            }
        }

        public void Shoot()
        {
            for (int i = 0; i < 4; i++)
            {
                var velocity = new Vector2(10, 0).RotatedBy(Projectile.rotation + i * MathHelper.PiOver2);

                Projectile.NewProjectile(data, Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
            }
        }

        List<Projectile> shotChains = new List<Projectile>();
        int chainLimit = 4;
        public void ShootChains()
        {
            float detectRange = MathF.Pow(800f, 2);

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC target = Main.npc[i];

                if (!target.CanBeChasedBy())
                {
                    continue;
                }

                var dist = Projectile.Center.DistanceSQ(target.Center);

                if (dist <= detectRange)
                {
                    var dir = (target.Center - Projectile.Center);
                    dir.Normalize();
                    dir *= 12;

                    Projectile chain = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, dir, ModContent.ProjectileType<PullChainProjectile>(), Projectile.damage / 2, 2, Projectile.owner);
                    shotChains.Add(chain);

                    if (shotChains.Count >= chainLimit)
                    {
                        break;
                    }
                }
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.rotation + 0.1f;

            if (shotChains.Count > 0 && shotChains.All((chain) => chain.timeLeft == 0))
            {
                Projectile.Kill();
            }

            base.AI();
        }
    }

    public class ChainedWaterProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ChainGuillotine);
            AIType = ProjectileID.ChainGuillotine;
        }

        Vector2 initVel = Vector2.Zero;
        public WaterGuns.ProjectileData data = null;
        WaterGunProjectile waterGun = null;
        public override void OnSpawn(IEntitySource source)
        {
            if (source is WaterGuns.ProjectileData newData)
            {
                data = newData;
            }
            else
            {
                data = new WaterGuns.ProjectileData(source);
            }

            initVel = Projectile.velocity;


            var proj = Projectile.NewProjectileDirect(data, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<WaterGunProjectile>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
            waterGun = proj.ModProjectile as WaterGunProjectile;

            base.OnSpawn(source);
        }

        public override void OnKill(int timeLeft)
        {
            if (!holdInPlace && waterGun != null)
            {
                waterGun.Projectile.Kill();
            }
            base.OnKill(timeLeft);
        }


        bool haveShot = false;
        bool holdInPlace = false;
        public override void AI()
        {
            base.AI();

            if (initVel != Projectile.velocity && !haveShot)
            {
                if (data.fullCharge)
                {
                    holdInPlace = true;
                    waterGun.ShootChains();
                }
                waterGun.Shoot();
                haveShot = true;
            }

            if (!holdInPlace)
                waterGun.Projectile.Center = new Vector2(Projectile.Center.X, Projectile.Center.Y - 4);
        }
    }
}
