using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules.Items;

// TODO seems like there's no reason for this to exist
// either make it more useful somehow, or remove it entirely
public class DataModule<TPlayer> : IModule
    where TPlayer : ModPlayer
{
    private TPlayer? _player = null;

    public TPlayer GetPlayer(Player player)
    {
        return _player ??= player.GetModPlayer<TPlayer>();
    }
}