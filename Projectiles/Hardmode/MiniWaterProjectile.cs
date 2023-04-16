using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public class TurretWaterProjectile : ModProjectile
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

        int delayMax = 10;
        int delay = 10;
        public override void AI()
        {
            base.AI();

            Player player = Main.player[Main.myPlayer];
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.TurretSummonBuff>());
            }
            if (player.HasBuff(ModContent.BuffType<Buffs.TurretSummonBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            var distanceToMouse = new Vector2(Main.MouseWorld.X - Projectile.Center.X, Main.MouseWorld.Y - Projectile.Center.Y);
            distanceToMouse.Normalize();
            Projectile.spriteDirection = (Main.MouseWorld.X - Projectile.Center.X > 0) ? 1 : -1;
            Projectile.rotation = Projectile.Center.AngleTo(Main.MouseWorld) - (Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi);

            if (Main.mouseLeft && delay >= delayMax)
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
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, 1f);
        }
    }
}
