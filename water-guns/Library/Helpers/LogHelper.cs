using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WaterGuns.Config;

namespace WaterGuns.Library.Helpers;

public static class LogHelper
{
    public static void Message(string text, string label = "", Color? color = null)
    {
        if (!ModContent.GetInstance<Configuration>().DebugInfoEnabled) return;
        
        color ??= Color.White;
        Main.NewText(label + text, color);
    }

    public static void Message(Vector2 vector, string label = "", Color? color = null)
    {
        if (!ModContent.GetInstance<Configuration>().DebugInfoEnabled) return;
        
        Message($"X: {vector.X}, Y: {vector.Y}", label, color);
    }

    public static void Log(string text, string label = "")
    {
        if (!ModContent.GetInstance<Configuration>().DebugInfoEnabled) return;
        
        ModContent.GetInstance<WaterGuns>().Logger.Info(label + text);
    }

    public static void Log(Vector2 vector, string label = "")
    {
        if (!ModContent.GetInstance<Configuration>().DebugInfoEnabled) return;
        
        Log($"X: {vector.X}, Y: {vector.Y}", label);
    }
}
