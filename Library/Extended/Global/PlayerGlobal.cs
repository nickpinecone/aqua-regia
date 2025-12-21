using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Global;

public class PlayerGlobal : ModPlayer
{
    public delegate void PostUpdateRunSpeedsDelegate(Player player);
    public static event PostUpdateRunSpeedsDelegate? PostUpdateRunSpeedsEvent;
    public override void PostUpdateRunSpeeds()
    {
        PostUpdateRunSpeedsEvent?.Invoke(Player);
    }

    public delegate void PreUpdateMovementDelegate(Player player);
    public static event PreUpdateMovementDelegate? PreUpdateMovementEvent;
    public override void PreUpdateMovement()
    {
        PreUpdateMovementEvent?.Invoke(Player);
    }
    
    public override void Unload()
    {
        PreUpdateMovementEvent = null;
        PostUpdateRunSpeedsEvent = null;
    }
}