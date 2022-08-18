using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class RainCloud : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 160;
            Projectile.friendly = false;
            Projectile.width = 54;
            Projectile.height = 24;
            Projectile.extraUpdates = 0;
        }

        int delay = 0;
        public override void AI()
        {
            delay += 1;
            if (delay > 10)
            {
                delay = 0;
                var modifiedVelocity = new Vector2(0, 14);
                var position = Projectile.Bottom + new Vector2(Main.rand.Next(-48, 48), 12);
                Projectile.NewProjectile(data, position, modifiedVelocity, ModContent.ProjectileType<SimpleWaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }
}
