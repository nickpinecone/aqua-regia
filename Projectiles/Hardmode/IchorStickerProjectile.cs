using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class IchorStickerProjectile : BaseProjectile
    {
        public bool second = false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.timeLeft += 120;
            Projectile.penetrate = -1;

            base.affectedByAmmoBuff = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            data.color = new Color(255, 250, 41);
            data.dustScale = 1.2f;
        }

        protected float gravity = 0.001f;
        public override void AI()
        {
            base.AI();
            gravity += 0.0012f;
            Projectile.velocity.Y += gravity;

            base.CreateDust();

            // Ichor dust that emits little light
            var dust2 = Dust.NewDust(Projectile.position, 5, 5, DustID.Ichor, 0, 0, 0, default, 0.8f);
            Main.dust[dust2].noGravity = true;
        }
    }
}
