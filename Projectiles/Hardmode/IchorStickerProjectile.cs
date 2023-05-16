using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.Hardmode
{
    public class MobStickerFriendly : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.height = 86;
            Projectile.width = 70;
            Main.projFrames[Projectile.type] = 4;
            Projectile.timeLeft = 180;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            Projectile.ai[0] = 10f;
            Projectile.alpha = 255;

            radius = new Vector2(Main.rand.Next(144, 196), 0);
            spinRot = Main.rand.Next(-180, 180);
            spinDegree = Main.rand.NextFloat(0.5f, 0.8f);
        }

        public NPC target;
        Vector2 radius;
        float spinRot;
        float spinDegree;

        int delay = 0;
        public override void AI()
        {
            base.AI();

            if (Projectile.ai[0] > 0f)
            {
                Projectile.ai[0] -= 1f;
                Projectile.alpha -= 255 / 10;
            }

            if (target != null)
            {
                if (target.GetLifePercent() <= 0f)
                {
                    Projectile.timeLeft = 10;
                    target = null;
                }
                else
                {
                    Projectile.Center = target.Center + radius.RotatedBy(MathHelper.ToRadians(spinRot));
                    spinRot += spinDegree;

                    delay += 1;
                    if (delay >= 25)
                    {
                        delay = 0;

                        var velocity = Projectile.Center.DirectionTo(target.Center);
                        velocity.Normalize();
                        velocity *= 7;

                        var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Projectile.Center, velocity, ModContent.ProjectileType<IchorStickerProjectile>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                        proj.tileCollide = false;
                    }
                }
            }


            if (++Projectile.frameCounter >= 12)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Projectile.timeLeft <= 10)
            {
                Projectile.alpha += 255 / 10;
            }
        }

    }

    public class IchorStickerProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.timeLeft += 120;
            Projectile.penetrate = 2;

            Projectile.usesIDStaticNPCImmunity = true;

            base.affectedByAmmoBuff = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.tileCollide = true;

            if (data.fullCharge)
            {
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MobStickerFriendly>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                (proj.ModProjectile as MobStickerFriendly).target = target;
            }

            target.AddBuff(BuffID.Ichor, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            data.color = new Color(255, 250, 41);
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
