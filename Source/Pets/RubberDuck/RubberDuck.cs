using AquaRegia.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Pets.RubberDuck;

public class RubberDuck : ModItem
{
    public override string Texture => TexturesPath.Pets + "RubberDuck/RubberDuck";

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.BlueEgg);

        Item.shoot = ModContent.ProjectileType<RubberDuckProjectile>();
        Item.buffType = ModContent.BuffType<RubberDuckBuff>();
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
