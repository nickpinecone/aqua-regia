using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class RainbowWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.affectedByHoming = false;

            AIType = ProjectileID.WaterGun;
            Projectile.penetrate = -1;
            Projectile.timeLeft += 30;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            base.data.alpha = 0;
        }

        float gravity = 0.1f;
        int delay = 0;
        public override void AI()
        {
            base.AI();
            Projectile.velocity.Y += gravity;

            Color newColor = new Color(Main.rand.Next(155, 255), Main.rand.Next(155, 255), Main.rand.Next(155, 255));
            base.data.color = newColor;

            delay += 1;
            if (delay >= 23)
            {
                delay = 0;
                var proj = Projectile.NewProjectileDirect(base.data, Projectile.Center, new Vector2(0, 10), ModContent.ProjectileType<Projectiles.Hardmode.WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                proj.width += 10;
            }

            base.CreateDust(default, data.dustScale, 3, 1, 0);
            Lighting.AddLight(Projectile.position, new Vector3(1f, 0.5f, 1f));
        }
    }
}
