using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class IchorWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.tileCollide = false;
            Projectile.timeLeft += 20;

            base.affectedByAmmoBuff = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 240);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            data.color = new Color(255, 250, 41);
            data.dustScale = 1.2f;
            data.dustAmount = 4;
        }

        int delay = 0;
        int delayMax = 5;
        public override void AI()
        {
            base.AI();
            base.CreateDust();

            if (!Projectile.tileCollide && Projectile.Center.Y >= Main.player[Main.myPlayer].Center.Y)
            {
                Projectile.tileCollide = true;
            }

            // Ichor dust that emits little light
            var dust2 = Dust.NewDust(Projectile.position, 5, 5, DustID.Ichor, 0, 0, 0, default, 0.8f);
            Main.dust[dust2].noGravity = true;
        }
    }
}
