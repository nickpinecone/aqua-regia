using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Pets
{
    public class DuckPetItem : ModItem
    {
        // Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BlueEgg); // Copy the Defaults of the Zephyr Fish Item.

            Item.shoot = ModContent.ProjectileType<DuckPetProjectile>(); // "Shoot" your pet projectile.
            Item.buffType = ModContent.BuffType<Buffs.DuckPetBuff>(); // Apply buff upon usage of the Item.
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(Item.buffType, 3600);
            }
            return true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            return;
        }
    }
}