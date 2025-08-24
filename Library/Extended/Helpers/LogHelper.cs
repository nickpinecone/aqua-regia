using AquaRegia.Config;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Helpers;

public static class LogHelper
{
    public static void Message(string text, string label = "", Color? color = null)
    {
        if (!ModContent.GetInstance<AquaConfig>().DebugInfoEnabled) return;

        color ??= Color.White;
        Main.NewText(label + text, color);
    }

    public static void Message(Vector2 vector, string label = "", Color? color = null)
    {
        if (!ModContent.GetInstance<AquaConfig>().DebugInfoEnabled) return;

        Message($"X: {vector.X}, Y: {vector.Y}", label, color);
    }

    public static void Log(string text, string label = "")
    {
        if (!ModContent.GetInstance<AquaConfig>().DebugInfoEnabled) return;

        ModContent.GetInstance<AquaRegia>().Logger.Info(label + text);
    }

    public static void Log(Vector2 vector, string label = "")
    {
        if (!ModContent.GetInstance<AquaConfig>().DebugInfoEnabled) return;

        Log($"X: {vector.X}, Y: {vector.Y}", label);
    }
}