using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode.MechanicalWaterProjectiles
{
    public class LauncherProjectile : MechanicalProjectile
    {
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            delay = 50;
            delayMax = 50;

            dist = -(Main.player[Main.myPlayer].Center - Projectile.Center);
            data.fullCharge = false;
            projType = ModContent.ProjectileType<Projectiles.Hardmode.WaterBalloonProjectile>();

            mouseLeftNeed = false;
            Projectile.timeLeft = 200;
        }
    }
}
