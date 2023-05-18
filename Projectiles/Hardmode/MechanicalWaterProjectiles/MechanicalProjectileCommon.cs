using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode.MechanicalWaterProjectiles
{
    public abstract class MechanicalProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            AIType = ProjectileID.WaterGun;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 10;
            Projectile.width = 102;
            Projectile.height = 48;
            base.affectedByAmmoBuff = false;
        }

        public Vector2 dist;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        public int delayMax = 40;
        public int delay = 40;
        public int timeOffset = 40;
        public bool spaz = false;
        public bool mouseLeftNeed = true;
        public int projType = 0;
        public override void AI()
        {
            base.AI();
            Projectile.Center = Main.player[Main.myPlayer].Center - dist;

            if (mouseLeftNeed)
            {
                Projectile.timeLeft = 10;
            }

            Projectile.spriteDirection = (Main.MouseWorld.X - Projectile.Center.X > 0) ? 1 : -1;
            Projectile.rotation = Projectile.Center.AngleTo(Main.MouseWorld) - (Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi);

            var distanceToMouse = new Vector2(Main.MouseWorld.X - Projectile.Center.X, Main.MouseWorld.Y - Projectile.Center.Y);
            distanceToMouse.Normalize();

            if ((Main.mouseLeft || !mouseLeftNeed) && delay >= delayMax)
            {
                delay = 0;
                var velocity = distanceToMouse * 10;
                var offset = new Vector2(Projectile.Center.X + velocity.X * 5, Projectile.Center.Y + velocity.Y * 5 + 2);
                var damage = Projectile.damage;

                if (spaz)
                {
                    base.data.dustAmount = 2;
                    base.data.dustScale = 2;
                    base.data.fadeIn = 0;
                    damage = (int)(damage / 1.2f);
                }
                else
                {
                    damage = (int)(damage * 2f);
                }

                var proj = Projectile.NewProjectileDirect(base.data, offset, velocity, projType, damage, Projectile.knockBack, Projectile.owner);
                proj.timeLeft += timeOffset;
            }
            else if (!Main.mouseLeft && mouseLeftNeed)
            {
                Projectile.Kill();
            }

            delay += 1;
        }
    }
}
