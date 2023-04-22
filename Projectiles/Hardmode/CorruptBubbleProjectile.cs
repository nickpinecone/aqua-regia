using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public class BubbledFish : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 180;

            Projectile.penetrate = -1;
            Projectile.tileCollide = true;

            Projectile.width = 26;
            Projectile.height = 22;
            Projectile.scale = 1.2f;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.alpha = 135;

            Projectile.usesIDStaticNPCImmunity = true;
        }

        int delay = 0;
        public override bool? CanHitNPC(NPC target)
        {
            if (hitTarget == null)
            {
                return base.CanHitNPC(target);
            }

            if (delay >= 18)
            {
                return base.CanHitNPC(target);
            }
            else
            {
                return false;
            }
        }

        NPC hitTarget = null;
        Vector2 hitPoint = Vector2.Zero;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            delay = 0;

            base.OnHitNPC(target, damage, knockback, crit);
            Projectile.velocity = Vector2.Zero;

            hitTarget = target;
            Projectile.alpha = 0;

            var dir = Projectile.position.DirectionTo(target.position);
            var dist = Projectile.position.Distance(target.position);

            hitPoint = dir * dist;
        }

        public override void AI()
        {
            base.AI();
            delay += 1;

            if (hitTarget != null)
            {
                Projectile.position = hitTarget.position - hitPoint;

                if (hitTarget.GetLifePercent() <= 0f)
                {
                    Projectile.Kill();
                }
            }
        }

    }

    public class CorruptBubbleProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 90;

            Projectile.penetrate = 1;
            Projectile.tileCollide = true;

            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 75;

            base.affectedByAmmoBuff = false;
        }


        Projectile bubbledFish = null;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Projectile.scale *= Main.rand.NextFloat(0.6f, 1f) + 1f;
            Projectile.velocity *= Main.rand.NextFloat(0.8f, 1.2f);

            var gun = (Main.player[Main.myPlayer].HeldItem.ModItem as Items.Hardmode.CorruptBubbleGun);

            if (gun.pumpShots > 0)
            {
                if (gun.pumpShots != 3)
                {
                    Projectile.scale *= 1.5f;
                }

                bubbledFish = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BubbledFish>(), Projectile.damage / 2, 0, Projectile.owner);
                bubbledFish.spriteDirection = Main.player[Main.myPlayer].direction;
                bubbledFish.rotation = Projectile.velocity.ToRotation() - (bubbledFish.spriteDirection == 1 ? 0 : MathHelper.Pi);

                gun.pumpShots -= 1;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (bubbledFish != null)
            {
                bubbledFish.Kill();
            }

            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            if (bubbledFish != null && timeLeft <= 0)
            {
                bubbledFish.Kill();
            }
            else if (bubbledFish != null && timeLeft >= 0)
            {
                bubbledFish.velocity = Projectile.velocity;
            }

            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit();

                var position = Projectile.Center;
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), 75, new Color(179, 252, 0), 0.8f);
                dust.noGravity = true;
                dust.velocity = speed * 6;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }


        public override void AI()
        {
            base.AI();

            if (bubbledFish != null)
            {
                bubbledFish.Center = Projectile.Center;
            }

            Projectile.velocity *= 0.984f;
        }
    }
}
