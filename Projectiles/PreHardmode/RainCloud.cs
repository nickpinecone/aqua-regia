using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class RainCloud : BaseProjectile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 120;
            Projectile.friendly = false;
            Projectile.width = 54;
            Projectile.height = 24;
            Projectile.extraUpdates = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            Projectile.ai[0] = 10f;
            Projectile.alpha = 255;
        }

        int delay = 0;
        public override void AI()
        {
            if (Projectile.ai[0] > 0f)
            {
                Projectile.ai[0] -= 1f;
                Projectile.alpha -= 255 / 10;
            }

            delay += 1;
            if (delay > 10)
            {
                delay = 0;
                var modifiedVelocity = new Vector2(0, 14);
                var position = Projectile.Bottom + new Vector2(Main.rand.Next(-42, 42), 12 - Main.rand.Next(0, 16));
                Projectile.NewProjectile(data, position, modifiedVelocity, ModContent.ProjectileType<SimpleWaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

            if (++Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Projectile.timeLeft <= 10)
            {
                Projectile.alpha += 255 / 10;
            }
        }
    }
}
