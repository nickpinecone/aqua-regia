using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;

namespace AquaRegia.Utils;

public static class ChatLog
{
    public static void Message(string text, Color? color = null)
    {
        color = color ?? Color.White;

        ChatHelper.DisplayMessage(NetworkText.FromLiteral(text), (Color)color, 1);
    }

    public static void Message(Vector2 vector, string label = "", Color? color = null)
    {
        Message(label + $"X: {vector.X}, Y: {vector.Y}", color);
    }
}
