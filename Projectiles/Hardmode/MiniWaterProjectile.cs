using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public class TurretWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 30;
            Projectile.timeLeft = 2;

            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        Vector2 originPosition;
        public override void OnSpawn(IEntitySource source)
        {
            originPosition = Projectile.Left;
            base.OnSpawn(source);
        }

        int delayMax = 12;
        int delay = 12;
        NPC target = null;
        public override void AI()
        {
            Player player = Main.player[Main.myPlayer];
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.TurretSummonBuff>());
            }
            if (player.HasBuff(ModContent.BuffType<Buffs.TurretSummonBuff>()))
            {
                Projectile.timeLeft = 10;
            }

            target = FindNearestNPC(600f);

            if (target != null)
            {
                var distanceToMouse = new Vector2(target.Center.X - Projectile.Center.X, target.Center.Y - Projectile.Center.Y);
                distanceToMouse.Normalize();
                Projectile.spriteDirection = (target.Center.X - Projectile.Center.X > 0) ? 1 : -1;
                Projectile.rotation = Projectile.Center.AngleTo(target.Center) - (Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi);

                if (delay >= delayMax)
                {
                    delay = 0;
                    var velocity = distanceToMouse * 10;
                    var offset = Projectile.Center + new Vector2(velocity.X * 3.3f, velocity.Y * 3.3f);
                    velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset, velocity, ModContent.ProjectileType<WaterProjectile>(), 40, 3, Projectile.owner);
                }
                delay += 1;
            }

        }
    }

    public class MiniWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;

            Projectile.timeLeft += 20;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            data.dustScale = 1f;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust();
        }
    }
}
