using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class CurveWaterProjectile : WaterProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.penetrate = -1;
        }

        float curveAmount = 0;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            curveAmount = Main.rand.NextFloat(-0.15f, 0.15f);
        }

        public override void AI()
        {
            base.AI();

            Projectile.velocity = Projectile.velocity.RotatedBy(curveAmount);
            curveAmount *= 1.02f;
        }
    }


    public class RainbowWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.affectedByHoming = false;

            AIType = ProjectileID.WaterGun;
            Projectile.penetrate = -1;
            Projectile.timeLeft += 30;
            Projectile.light = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            base.data.alpha = 0;
            data.fadeIn = 1;
            data.alpha = 0;
        }

        float gravity = 0.1f;
        int delay = 0;
        int delayCurve = 0;
        public override void AI()
        {
            base.AI();
            Projectile.velocity.Y += gravity;

            Color newColor = new Color(Main.rand.Next(155, 255), Main.rand.Next(155, 255), Main.rand.Next(155, 255));
            base.data.color = newColor;

            delay += 1;
            delayCurve += 1;

            if (data.fullCharge)
            {
                if (delayCurve >= 16)
                {
                    var proj = Projectile.NewProjectileDirect(base.data, Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.Hardmode.CurveWaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    delayCurve = 0;
                }
            }

            else
            {
                if (delay >= 23)
                {
                    delay = 0;
                    var proj = Projectile.NewProjectileDirect(base.data, Projectile.Center, new Vector2(0, 10), ModContent.ProjectileType<Projectiles.Hardmode.WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    proj.light = 1f;
                    proj.width += 10;
                }
            }

            base.CreateDust();
        }
    }
}
