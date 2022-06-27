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
        }

        bool turretMode = false;
        Projectile turret = null;
        public override bool AltFunctionUse(Player player)
        {
            turretMode = !turretMode;
            if (turretMode)
            {
                turret = Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hardmode.TurretWaterProjectile>(), 0, 0, player.whoAmI);
            }
            else
            {
                turret.Kill();
            }
            return base.AltFunctionUse(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!turretMode)
            {
                Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
                var offset = new Vector2(position.X + velocity.X * 4, position.Y + velocity.Y * 4);
                Projectile.NewProjectile(source, offset, modifiedVelocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
