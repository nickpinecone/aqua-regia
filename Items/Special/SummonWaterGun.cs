using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.Special
{
    public abstract class SummonWaterGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ImpStaff);
            Item.shoot = ModContent.ProjectileType<Projectiles.Special.WaterGunSummon>();
            Item.damage = 66;
            Item.buffType = ModContent.BuffType<Buffs.WaterGunSummonBuff>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2, true);
            position = Main.MouseWorld;
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
