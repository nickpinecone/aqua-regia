using AquaRegia.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Pets;

public class DuckItem : ModItem
{
    public override string Texture => TexturesPath.Pets + "DuckItem";

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.BlueEgg);

        Item.shoot = ModContent.ProjectileType<DuckProjectile>();
        Item.buffType = ModContent.BuffType<DuckBuff>();
        Item.value = Item.buyPrice(0, 5, 0, 0);
        Item.rare = ItemRarityID.Blue;
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            player.AddBuff(Item.buffType, 2);
        }
        return true;
    }
}
