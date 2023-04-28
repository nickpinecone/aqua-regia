using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class ChlorophyteWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;

            Projectile.timeLeft += 20;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            data.dustScale = 1;
        }

        int delay = 0;
        int delayMax = Main.rand.Next(0, 40);
        public override void AI()
        {
            base.AI();
            base.CreateDust();
            base.AutoAim();

            delay += 1;
            if (delay >= delayMax)
            {
                delay = 0;
                delayMax = Main.rand.Next(30, 80);

                var position = Projectile.Center + new Vector2(Main.rand.Next(-30, 30), Main.rand.Next(-30, 30));
                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), position, Vector2.Zero, ProjectileID.SporeTrap, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

        }
    }
}
