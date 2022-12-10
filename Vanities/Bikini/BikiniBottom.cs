using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Vanities.Bikini
{
    [AutoloadEquip(EquipType.Legs)]
    public abstract class BikiniBottom : Armors.BaseArmors.BasePants
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

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            color = Main.player[Main.myPlayer].pantsColor;
            base.DrawArmorColor(drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
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
