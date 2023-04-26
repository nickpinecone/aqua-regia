using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;

namespace WaterGuns.Projectiles.PreHardmode
{
    public abstract class BaseProjectile : CommonWaterProjectile
    {
        public bool defaultDust = true;
        public bool applyGravity = true;

        public override void AI()
        {
            // Curve it like the in-game water gun projectile
            if (applyGravity)
            {
                gravity += 0.002f;
                Projectile.velocity.Y += gravity;
            }

            // The dust should be created in the child class
            if (defaultDust)
                CreateDust();

            if (data.homesIn)
                AutoAim();

            base.AI();
        }
    }
}
