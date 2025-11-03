using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Global;

// TODO Something like this, can extend
// maybe, dont know if we even need that
public class PlayerGlobal : ModPlayer
{
    public delegate void PostUpdateRunSpeedsDelegate(Player player);
    public static event PostUpdateRunSpeedsDelegate? PostUpdateRunSpeedsEvent;
    public override void PostUpdateRunSpeeds()
    {
        PostUpdateRunSpeedsEvent?.Invoke(Player);
    }
}