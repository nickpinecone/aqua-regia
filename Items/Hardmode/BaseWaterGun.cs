using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Hardmode
{
    public abstract class BaseWaterGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterGun);
            Item.useTime -= 2;
            Item.useAnimation -= 2;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // All of them use custom projectiles that shoot straight 
            // Make them a little inaccurate like in-game water gun
            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(1));
            // Custom projectile doesnt position right so offset it
            var offset = new Vector2(position.X + velocity.X * 4, position.Y + velocity.Y * 4);
            Projectile.NewProjectile(source, offset, modifiedVelocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 4);
        }
    }
}
