using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public class MiniWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Rapid but inaccurate. Press right click to turn into a turret");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 45;
            Item.knockBack = 4;

            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hardmode.MiniWaterProjectile>();
            base.defaultInaccuracy = 8;
        }

        Projectile turret = null;
        public override bool AltFunctionUse(Player player)
        {
            if (player.HasBuff<Buffs.TurretSummonBuff>())
            {
                player.ClearBuff(ModContent.BuffType<Buffs.TurretSummonBuff>());
            }
            else
            {
                player.AddBuff(ModContent.BuffType<Buffs.TurretSummonBuff>(), 20);
                turret = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.TurretWaterProjectile>(), 0, 0, player.whoAmI);
            }
            return base.AltFunctionUse(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!player.HasBuff<Buffs.TurretSummonBuff>())
            {
                base.SpawnProjectile(player, source, position, velocity, type, damage, knockback);
            }
            return false;
        }
    }
}
