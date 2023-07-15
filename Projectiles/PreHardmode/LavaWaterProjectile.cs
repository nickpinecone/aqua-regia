using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class FireBreath : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultDust = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft -= 30;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 240);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void AI()
        {
            base.AI();

            // Fire dust that emits light
            var dust2 = Dust.NewDust(Projectile.position, 10, 10, DustID.Flare, 0, 0, 0, default, 3f);
            Main.dust[dust2].noGravity = true;
        }
    }

    public class LavaWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.defaultDust = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 240);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            data.color = new Color(255, 215, 50);
        }

        public override void AI()
        {
            base.AI();

            // Creating some dust to see the projectile
            base.CreateDust();

            // Fire dust that emits light
            var dust2 = Dust.NewDust(Projectile.position, 5, 5, DustID.Flare, 0, 0, 0, default, 3f);
            Main.dust[dust2].noGravity = true;
        }
    }
}
