using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Accessories.FrogHat;
using WaterGuns.Ammo;
using WaterGuns.Weapons.Shotgun;

namespace WaterGuns.Global;

public class NpcGlobal : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        base.ModifyShop(shop);

        if (shop.NpcType == NPCID.Merchant)
        {
            shop.Add(ModContent.ItemType<BottledWater>());
            shop.Add(ModContent.ItemType<FrogHat>());
            shop.Add(ModContent.ItemType<Shotgun>(), Condition.DownedKingSlime);
        }
    }
}
