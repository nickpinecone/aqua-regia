using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class CursedWaterProjectile : BaseProjectile
    {
        public bool second = false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.height == 8)
            {
                for (int i = -1; i < 2; i += 2)
                {
                    // Determines from what side the new projectile spawns
                    int direction = i;

                    // Speed it up a bit
                    int projectileSpeed = 10;
                    var modifiedVelocity = new Vector2(1 * direction, 0).RotatedBy(MathHelper.ToRadians(225 * -direction)).RotatedByRandom(MathHelper.ToRadians(2));
                    modifiedVelocity *= projectileSpeed;

                    // Offset from the target 
                    var offset = new Vector2(Projectile.position.X + (196 + Main.rand.Next(-5, 5)) * direction, Projectile.position.Y - (196 + Main.rand.Next(-5, 5)));

                    // Spawn default water projectile
                    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), offset, modifiedVelocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    proj.tileCollide = false;
                    proj.height -= 1;
                }
            }

            target.AddBuff(BuffID.CursedInferno, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            base.AI();

            // Creating some dust to see the projectile
            var offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            offset.Normalize();
            offset *= 3;

            for (int i = 0; i < 4; i++)
            {
                var position = new Vector2(Projectile.Center.X + offset.X * i, Projectile.Center.Y + offset.Y * i);
                var dust = Dust.NewDustPerfect(position, DustID.Wet, new Vector2(0, 0), 0, new Color(0, 255, 0), 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 1;
            }
        }
    }
}
