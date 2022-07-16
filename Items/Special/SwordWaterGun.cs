using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Special
{
    public class SwordData : IEntitySource
    {
        public string context = "";
        public string Context { get { return context; } }

        public int direction = 0;
    }

    public abstract class SwordWaterGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenSword);
            Item.damage = 66;
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

                var velocity = new Vector2(0, -12).RotatedBy(MathHelper.ToRadians(15 * -player.direction)).RotatedBy(MathHelper.ToRadians(shot * 12 * player.direction));
                var offset = player.Center + velocity * 4;

                var data = new SwordData();
                data.context = Projectile.GetSource_TownSpawn().Context;
                data.direction = player.direction;

                Projectile.NewProjectile(data, offset, velocity, ModContent.ProjectileType<Projectiles.Special.SwordWaterProjectile>(), Item.damage, Item.knockBack, player.whoAmI);
            }
            return base.UseItem(player);
        }
    }
}
