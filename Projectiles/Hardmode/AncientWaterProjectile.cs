using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class GeyserPlatform : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 96;
            Projectile.height = 48;
            Projectile.timeLeft = 60;
        }
    }

    public class AncientGeyser : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 64;
            Projectile.height = 448;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.AddBuff(BuffID.OnFire3, 60);
        }

        public override void AI()
        {
            base.AI();
            if (Projectile.ai[0] == 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Bottom + new Vector2(0, 24), Vector2.Zero, ModContent.ProjectileType<GeyserPlatform>(), 0, 0, Projectile.owner);
                Projectile.ai[0] = 0;
            }
        }
    }

    public class AncientWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust();
        }
    }
}
