using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Global
{
    public class GlobalItemList : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.WaterGun)
            {
                item.damage = 1;
                item.autoReuse = true;
            }
        }
    }
}
