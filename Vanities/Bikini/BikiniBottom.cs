using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Vanities.Bikini
{
    [AutoloadEquip(EquipType.Legs)]
    public class BikiniBottom : Armors.BaseArmors.BasePants
    {
        string femaleTextureName = "BikiniBottomFemale";

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
            DisplayName.SetDefault("Shorts");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 20;
            Item.height = 20;
        }
    }
}
