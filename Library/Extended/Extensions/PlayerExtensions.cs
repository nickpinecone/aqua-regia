using AquaRegia.Library.Extended.Data;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AquaRegia.Library.Extended.Extensions;

public static class PlayerExtensions
{
    public static SpriteEffects ToHorizontalFlip(this Player player)
    {
        return player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    }

    public static void SetBodyFrame(this Player player, PlayerFrames frame)
    {
        player.bodyFrame.Y = player.bodyFrame.Height * (int)frame;
    }

    public static void SetLegFrame(this Player player, PlayerFrames frame)
    {
        player.legFrame.Y = player.legFrame.Height * (int)frame;
    }

    public static PlayerFrames GetBodyFrame(this Player player)
    {
        return (PlayerFrames)(player.bodyFrame.Y / player.bodyFrame.Height);
    }

    public static PlayerFrames GetLegFrame(this Player player)
    {
        return (PlayerFrames)(player.legFrame.Y / player.legFrame.Height);
    }
}