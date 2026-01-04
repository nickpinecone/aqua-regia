using System.Collections.Generic;
using System.Linq;
using AquaRegia.Config;
using AquaRegia.Library.Extended.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Helpers;

// DustID does not have these
public static class DustExID
{
    public const short Wood = 7;
    public const short BreathBubble = 34;
}

public static class DustHelper
{
    public static void Debug(Vector2 position, Color? color = null)
    {
        if (!ModContent.GetInstance<AquaConfig>().DebugInfoEnabled) return;

        Dust.QuickDust(position, color ?? Color.Red);
    }

    public static void Debug(Point point, Color? color = null)
    {
        if (!ModContent.GetInstance<AquaConfig>().DebugInfoEnabled) return;

        Dust.QuickDust(point, color ?? Color.Red);
    }

    public static Dust Single(int type, Vector2 position, Vector2 size, Vector2 velocity, float scale = 1f,
        int alpha = 0, Color color = default)
    {
        return Dust.NewDustDirect(position, (int)size.X, (int)size.Y, type, velocity.X, velocity.Y, alpha, color,
            scale);
    }

    public static Dust SinglePerfect(int type, Vector2 position, Vector2 velocity, float scale = 1f, int alpha = 0,
        Color color = default)
    {
        return Dust.NewDustPerfect(position, type, velocity, alpha, color, scale);
    }

    public static IEnumerable<Dust> GenerateArc(bool isPerfect, bool isEven, int type, Vector2 position, Vector2 size,
        Vector2 startVector, Vector2 endVector, int amount, float speed,
        float scale, float offset, int alpha, Color color)
    {
        startVector = startVector.SafeNormalize(Vector2.Zero);
        endVector = endVector.SafeNormalize(Vector2.Zero);

        var angle = startVector.AnglePositive(endVector);
        var angleStep = angle / (amount - (isEven ? 1 : 0));

        for (var i = 0; i < amount; i++)
        {
            yield return isPerfect switch
            {
                true => SinglePerfect(type, position + startVector * offset, startVector * speed, scale, alpha, color),
                _ => Single(type, position + startVector * offset, size, startVector * speed, scale, alpha, color)
            };

            startVector = startVector.RotatedBy(angleStep);
        }
    }
}