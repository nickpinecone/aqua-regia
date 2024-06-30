using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;

namespace WaterGuns.Utils;

public static class ChatLog
{
    public static void Message(string text, Color? color = null)
    {
        color = color ?? Color.White;

        ChatHelper.DisplayMessage(NetworkText.FromLiteral(text), (Color)color, 1);
    }
}
