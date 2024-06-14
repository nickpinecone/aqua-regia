using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Global;

class GlobalItemList : GlobalItem
{
    public override void SetDefaults(Item entity)
    {
        base.SetDefaults(entity);

        if (entity.type == ItemID.BottledWater)
        {
            entity.ammo = ItemID.BottledWater;
        }
    }
}