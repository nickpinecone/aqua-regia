using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Projectiles.Special
{
    public class WaterGunSummon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Gun Minion");
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            // Denotes that this projectile is a pet or minion
            Main.projPet[Projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 34;
            Projectile.height = 26;
            Projectile.aiStyle = 62;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
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
            Player player = Main.player[Main.myPlayer];
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Debuffs.WaterGunSummonBuff>());
            }
            if (player.HasBuff(ModContent.BuffType<Debuffs.WaterGunSummonBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            float leastDist = float.PositiveInfinity;
            var vector = Vector2.Zero;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                var dist = Projectile.Distance(Main.npc[i].position);
                var distance = Main.player[Main.myPlayer].position - Main.npc[i].position;
                bool isVisible = Math.Abs(distance.X) < Main.ViewSize.X && Math.Abs(distance.Y) < Main.ViewSize.Y;

                if (Main.npc[i].life > 0 && isVisible && dist < leastDist)
                {
                    leastDist = dist;
                    vector = Main.npc[i].position - Projectile.position;

                    Projectile.rotation = Projectile.Center.AngleTo(Main.npc[i].position) - (Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi);
                }
            }

            delay += 1;
            if (delay >= 30)
            {
                delay = 0;

                if (vector != Vector2.Zero)
                {
                    vector.Normalize();
                    var offset = Projectile.Center + vector * 10;
                    Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), offset, vector * 10, ModContent.ProjectileType<Projectiles.Hardmode.WaterProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }

            base.AI();
        }
    }
}
