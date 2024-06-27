using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Global;

public class GlobalItemList : GlobalItem
{
    public override void SetDefaults(Item entity)
    {
        base.SetDefaults(entity);

        if (entity.type == ItemID.BottledWater)
        {
            entity.ammo = ItemID.BottledWater;
            entity.DamageType = DamageClass.Ranged;
            entity.damage = 1;
        }
    }
}
