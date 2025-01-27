using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Global;

public class ItemGlobal : GlobalItem
{
    public delegate void ModifyItemLootDelegate(Item item, ItemLoot itemLoot);
    public static event ModifyItemLootDelegate? ModifyItemLootEvent;
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        ModifyItemLootEvent?.Invoke(item, itemLoot);
    }
}
