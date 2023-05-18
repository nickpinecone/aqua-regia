using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode.MechanicalWaterProjectiles
{
    public class RetinazerProjectile : MechanicalProjectile
    {
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            dist = Main.player[Main.myPlayer].Center - Projectile.Center;
            projType = ModContent.ProjectileType<Projectiles.Hardmode.WaterProjectile>();
        }
    }
}
