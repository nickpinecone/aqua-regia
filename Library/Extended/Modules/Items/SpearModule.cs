using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AquaRegia.Library.Extended.Modules.Items;

public class SpearModule : IModule, IItemRuntime
{
    public void RuntimeSetStaticDefaults(BaseItem item)
    {
        ItemID.Sets.SkipsInitialUseSound[item.Type] = true;
        ItemID.Sets.Spears[item.Type] = true;
    }

    public bool RuntimeCanUseItem(BaseItem item, Player player)
    {
        return player.ownedProjectileCounts[item.Item.shoot] < 1;
    }

    public bool? RuntimeUseItem(BaseItem item, Player player)
    {
        if (item.Item.UseSound.HasValue)
        {
            SoundEngine.PlaySound(item.Item.UseSound.Value, player.Center);
        }

        return null;
    }
}