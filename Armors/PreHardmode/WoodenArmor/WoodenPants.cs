using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Armors.PreHardmode.WoodenArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class WoodenPants : BaseArmors.BasePants
    {
        string femaleTextureName = "WoodenPantsFemale";

        public override void Load()
        {
            base.Load();
            EquipLoader.AddEquipTexture(Mod, $"{Texture}Female_{EquipType.Legs}", EquipType.Legs, name: femaleTextureName);
        }

        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            if (!player.Male)
            {
                player.GetModPlayer<GlobalPlayer>().isFemaleLegs = true;
                player.GetModPlayer<GlobalPlayer>().femaleLegsTexture = femaleTextureName;
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            if (!player.Male)
            {
                player.GetModPlayer<GlobalPlayer>().isFemaleLegs = true;
                player.GetModPlayer<GlobalPlayer>().femaleLegsTexture = femaleTextureName;
            }
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Wooden Shorts");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 20;
            Item.height = 20;
            Item.defense = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddIngredient(ItemID.Acorn, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
