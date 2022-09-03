using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Armors.BaseArmors
{
    [AutoloadEquip(EquipType.Legs)]
    public abstract class BasePants : ModItem
    {
        public string femaleTextureName = "";

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
    }
}
