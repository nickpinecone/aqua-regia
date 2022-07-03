using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns
{
    public class GlobalPlayer : ModPlayer
    {
        // 0 = stat not changed, 1 = stat maxed
        public float waterGunAccuracy = 0;
        public float waterGunSpeed = 0;
        public float waterGunRange = 0;
        public bool waterGunShield = false;

        public float CalculateAccuracy(float defaultInaccuracy)
        {
            return defaultInaccuracy - defaultInaccuracy * waterGunAccuracy;
        }

        public float CalculateSpeed()
        {
            return 1 + waterGunSpeed;
        }

        public float CalculateRange(float defaultRange)
        {
            return defaultRange + defaultRange * waterGunRange;
        }

        public void ReleaseBlastShield()
        {
            int numOfProjs = 8;
            var player = Main.player[Main.myPlayer];
            if (waterGunShield)
            {
                for (int i = 0; i < numOfProjs; i++)
                {
                    var velocity = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(i * (360 / numOfProjs)));
                    velocity *= 10 * CalculateSpeed();
                    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.Center, velocity, ModContent.ProjectileType<Projectiles.Hardmode.SoundwaveProjectile>(), 40, 8, player.whoAmI);
                    proj.tileCollide = false;
                    proj.penetrate = -1;
                    proj.timeLeft = 60;
                }
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            ReleaseBlastShield();
            base.OnHitByNPC(npc, damage, crit);
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            ReleaseBlastShield();
            base.OnHitByProjectile(proj, damage, crit);
        }

        public override void ResetEffects()
        {
            waterGunAccuracy = 0;
            waterGunSpeed = 0;
            waterGunRange = 0;
            waterGunShield = false;
        }
    }
}
