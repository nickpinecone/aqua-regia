using AquaRegia.Weapons.Ice;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Global;

public class ItemGlobal : GlobalItem
{
    public override void ModifyItemLoot(Terraria.Item item, ItemLoot itemLoot)
    {
        base.ModifyItemLoot(item, itemLoot);

        if (item.type == ItemID.DeerclopsBossBag)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceGun>(), 2));
        }
    }
}
