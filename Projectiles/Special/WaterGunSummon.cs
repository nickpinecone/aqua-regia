using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Special
{
    public class WaterGunSummon : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 34;
            Projectile.height = 26;
            Projectile.aiStyle = 62;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        int delay = 0;
        public override void AI()
        {

            if (delay >= 10)
            {

                float leastDist = 0;
                var vector = Vector2.Zero;
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    var dist = Projectile.Distance(Main.npc[i].position);
                    if (i == 0)
                    {
                        leastDist = dist;
                        vector = Projectile.position - Main.npc[i].position;
                    }
                    else if (dist < leastDist)
                    {
                        leastDist = dist;
                        vector = Projectile.position - Main.npc[i].position;
                    }
                }

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vector * 10, ProjectileID.WaterGun, Projectile.damage, Projectile.knockBack, Projectile.owner);

                delay = 0;
            }
            delay += 1;

            base.AI();
        }
    }
}
