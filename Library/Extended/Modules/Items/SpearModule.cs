using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AquaRegia.Library.Extended.Modules.Items;

public class SpearModule : IModule, IItemRuntime
{
    public void SetStaticDefaults(BaseItem baseItem)
    {
        ItemID.Sets.SkipsInitialUseSound[baseItem.Type] = true;
        ItemID.Sets.Spears[baseItem.Type] = true;
    }

    public bool CanUseItem(BaseItem baseItem, Player player)
    {
        return player.ownedProjectileCounts[baseItem.Item.shoot] < 1;
    }

    public void PlayUseSound(BaseItem baseItem, Player player)
    {
        if (baseItem.Item.UseSound.HasValue)
        {
            SoundEngine.PlaySound(baseItem.Item.UseSound.Value, player.Center);
        }
    }

    public void RuntimeSetStaticDefaults(BaseItem baseItem)
    {
        SetStaticDefaults(baseItem);
    }

    public bool RuntimeCanUseItem(BaseItem baseItem, Player player)
    {
        return CanUseItem(baseItem, player);
    }

    public bool? RuntimeUseItem(BaseItem baseItem, Player player)
    {
        PlayUseSound(baseItem, player);

        return null;
    }
}