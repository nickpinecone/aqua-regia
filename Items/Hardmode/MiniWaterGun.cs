using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class MiniWaterGunBoomerang : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.penetrate = -1;
            Projectile.width = 46;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            returnToPlayer = true;
            return false;
        }

        float flyTime = 0;
        public bool returnToPlayer = false;
        float speed = 14;
        public override void AI()
        {
            base.AI();

            flyTime += 1;

            if (flyTime > 40)
            {
                returnToPlayer = true;
            }

            if (returnToPlayer)
            {
                Projectile.tileCollide = false;
                var dir = Projectile.Center.DirectionTo(Main.player[Main.myPlayer].Center);
                Projectile.velocity = dir * speed;
                speed *= 1.02f;

            }
            else
            {
                Projectile.velocity *= 1.02f;
            }


            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += 0.3f;
            }
            else if (Projectile.velocity.X < 0)
            {
                Projectile.rotation -= 0.3f;
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.Red;

            return base.PreDraw(ref lightColor);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.AddBuff(BuffID.OnFire, 60 * 2);

            if (!returnToPlayer)
            {
                returnToPlayer = true;
            }

            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.WaterExplosion>(), damage, knockback, Projectile.owner);

            for (int i = 0; i < 8; i++)
            {
                var speed = Main.rand.NextVector2Unit();
                var dust = Dust.NewDust(Projectile.Center, 16, 16, DustID.Smoke, 0, 0, 75, default, 3f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = speed * Main.rand.Next(2, 7);
            }
            for (int i = 0; i < 16; i++)
            {
                var speed = Main.rand.NextVector2Unit();
                var dust = Dust.NewDust(Projectile.Center, 16, 16, DustID.Flare, 0, 0, 75, default, 3f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = speed * Main.rand.Next(2, 7);
            }
        }

    }

    public class MiniWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Mini Water Gun");
            Tooltip.SetDefault("Rapid but inaccurate\nRight click to place a turret\nFull Pump: Throws the weapon from overheat which creates a small explosion on contact\nDrops from SantaNK1");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 51;
            Item.knockBack = 4;

            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.MiniWaterProjectile>();
            base.defaultInaccuracy = 8;

            base.offsetAmount = new Vector2(6, 6);
            base.offsetIndependent = new Vector2(0, 14);

            base.maxPumpLevel = 80;
        }

        float timeSinceShot = 0;
        float coolScale = 1;
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
            timeSinceShot += 1;

            var percent = 255 / base.maxPumpLevel * base.pumpLevel;
            Item.color = new Color(255, 255 - percent, 255 - percent);

            if (pumpLevel > 0)
            {
                if (timeSinceShot > 60)
                {
                    pumpLevel -= (int)(1 * coolScale);
                    if (pumpLevel < 0)
                    {
                        pumpLevel = 0;
                    }
                    coolScale *= 1.005f;
                }
            }

            if (boomerang != null && boomerang.returnToPlayer && boomerang.Projectile.Center.Distance(Main.player[Main.myPlayer].Center) < 64f)
            {
                boomerang.Projectile.Kill();
                boomerang = null;
                Item.useStyle = 5;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        public override bool AltFunctionUse(Player player)
        {
            if (player.HasBuff<Buffs.TurretSummonBuff>())
            {
                turret.Center = player.Center;
            }
            else
            {
                player.AddBuff(ModContent.BuffType<Buffs.TurretSummonBuff>(), 60);
                turret = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.TurretWaterProjectile>(), 0, 0, player.whoAmI);
            }

            return base.AltFunctionUse(player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-14, 14);
        }

        Projectile turret = null;
        MiniWaterGunBoomerang boomerang = null;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                var proj = Projectile.NewProjectileDirect(source, position, velocity * 1.5f, ModContent.ProjectileType<MiniWaterGunBoomerang>(), damage * 3, knockback, player.whoAmI);
                boomerang = (proj.ModProjectile as MiniWaterGunBoomerang);
                Item.useStyle = 0;
                pumpLevel = 0;
            }

            else
            {
                timeSinceShot = 0;

                if (pumpLevel < maxPumpLevel)
                {
                    pumpLevel += 1;
                }

                var proj = base.SpawnProjectile(player, source, position, velocity, type, damage, knockback);

                player.itemRotation = proj.velocity.ToRotation() - (player.direction == -1 ? MathHelper.Pi : 0);
            }

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(NetworkText.FromLiteral("Mods.WaterGuns.Conditions.Never"), (_) => false);
            recipe.Register();
        }
    }
}
