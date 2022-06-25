using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public class RocketWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RocketI);
            AIType = ProjectileID.RocketI;
            Projectile.damage = 1;
            Projectile.timeLeft = 120;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                var velocity = new Vector2(10, 0).RotatedByRandom(MathHelper.ToRadians(180));
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Projectile.Center, velocity, ModContent.ProjectileType<WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                proj.tileCollide = false;
            }
            Projectile.timeLeft = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            this.Kill(0);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            base.AutoAim();
            base.AI();
        }
    }
}
