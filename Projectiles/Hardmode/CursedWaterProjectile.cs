using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class CursedWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            hasKillEffect = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.height == 16)
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

        public override void Kill(int timeLeft)
        {
            base.CreateKillEffect(new Color(96, 248, 2), 0.8f);
            base.Kill(timeLeft);
        }

        public override void AI()
        {
            base.AI();

            base.CreateDust(new Color(96, 248, 2), 1.2f);

            // Cursed Flame dust that emits light
            var dust2 = Dust.NewDust(Projectile.position, 5, 5, DustID.CursedTorch, 0, 0, 0, default, 1.6f);
            Main.dust[dust2].fadeIn = 1.2f;
            Main.dust[dust2].noGravity = true;
        }
    }
}
