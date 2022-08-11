using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.Hardmode
{
    public abstract class BaseProjectile : CommonWaterProjectile
    {
        protected bool affectedByBounce = true;
        protected bool affectedByHoming = true;

        public override void AI()
        {
            if (data.homesIn && affectedByHoming)
            {
                AutoAim();
            }
            base.AI();
        }
    }
}
