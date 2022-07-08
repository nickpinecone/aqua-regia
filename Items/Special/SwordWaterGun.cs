using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Special
{
    public class SwordWaterGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenSword);
            Item.autoReuse = true;
        }

        int delay = 3;
        int shot = 0;
        public override bool? UseItem(Player player)
        {
            if (shot >= 9)
            {
                shot = 0;
                delay = 3;
            }
            delay += 1;
            if (delay >= 3)
            {
                shot += 1;
                delay = 0;

                var velocity = new Vector2(0, -12).RotatedBy(MathHelper.ToRadians(shot * -12));
                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), player.Center, velocity, ModContent.ProjectileType<Projectiles.Special.SwordWaterProjectile>(), Item.damage, Item.knockBack, player.whoAmI);
            }
            return base.UseItem(player);
        }
    }
}
