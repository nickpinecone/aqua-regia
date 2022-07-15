using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class BubbleProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bubble);
            AIType = ProjectileID.Bubble;
        }

        public override void OnSpawn(IEntitySource source)
        {
            int random = Main.rand.Next(-5, 5);
            randomOffset1 = random;

            random = Main.rand.Next(-5, 5);
            randomOffset2 = random;

            base.OnSpawn(source);
        }

        int delay = 0;
        int randomOffset1 = 0;
        int randomOffset2 = 0;
        public override void AI()
        {
            delay += 1;
            if (delay < 20 + randomOffset1)
            {
                Projectile.position += new Vector2(1.5f, 0);
            }
            else if (delay < 40 + randomOffset2)
            {
                Projectile.position += new Vector2(-1.5f, 0);
            }
            else
            {
                delay = 0;
            }

            base.AI();
        }
    }

    public class OceanWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            // Projectile.CloneDefaults(ProjectileID.WaterGun);
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            // Offset randomly
            var offset = new Vector2();
            offset.X = Projectile.position.X + Main.rand.Next(-60, 60);
            offset.Y = Projectile.position.Y + Main.rand.Next(0, 60);

            // Dont know how to extract IEventSource from BubbleProjectile so using OceanWaterProjectile source
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset, new Vector2(0, -4), ModContent.ProjectileType<BubbleProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
