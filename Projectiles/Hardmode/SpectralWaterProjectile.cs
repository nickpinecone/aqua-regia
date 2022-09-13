using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Projectiles.Hardmode
{
    public class SoulProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.LostSoulFriendly);
            AIType = ProjectileID.LostSoulFriendly;
            Projectile.friendly = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            spin = new Vector2(Main.rand.Next(24, 72), 0);
            spinSpeed = Main.rand.NextBool() ? -1.6f : 1.6f;
            base.OnSpawn(source);
        }

        Vector2 spin = new Vector2(0, 0);
        float spinSpeed = 0f;
        public override void AI()
        {
            if (Projectile.friendly == false)
            {
                Projectile.velocity = Vector2.Zero;
            }


            if (Projectile.velocity != Vector2.Zero)
            {
                base.AI();
            }
            else
            {
                spin = spin.RotatedBy(MathHelper.ToRadians(spinSpeed));
                Projectile.position = Main.player[Main.myPlayer].position + spin;
            }
        }
    }

    public class SpectralWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.timeLeft += 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.player[Projectile.owner].HeldItem.ModItem is Items.Hardmode.SpectralWaterGun gun)
            {
                if (gun.pumpLevel < 10)
                {
                    gun.pumpLevel += 1;
                    gun.hitCount += 1;
                    if (gun.soulsList.Count < 10)
                    {
                        gun.hitCount = 0;
                        var player = Main.player[Main.myPlayer];
                        var proj = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.position, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.Hardmode.SoulProjectile>(), gun.Item.damage, 2, player.whoAmI);
                        gun.soulsList.Add(proj);
                    }
                }
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            base.AI();
            base.CreateDust(default, 1f);
        }
    }
}
