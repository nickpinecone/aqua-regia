using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WaterGuns.Config;
using WaterGuns.Library.Extensions;

namespace WaterGuns.Library.Helpers;

// DustID does not have these
public static class ParticleID
{
    public const short Wood = 7;
    public const short BreathBubble = 34;
}

public static class DustHelper
{
    public static void Debug(Vector2 position, Color? color = null)
    {
        if (!ModContent.GetInstance<Configuration>().DebugInfoEnabled) return;
        
        Dust.QuickDust(position, color ?? Color.Red);
    }

    public static void Debug(Point point, Color? color = null)
    {
        if (!ModContent.GetInstance<Configuration>().DebugInfoEnabled) return;
        
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

    private static IEnumerable<Dust> GenerateArc(bool isPerfect, bool isEven, int type, Vector2 position, Vector2 size,
                                                 Vector2 startVector, Vector2 endVector, int amount, float speed,
                                                 float scale, float offset, int alpha, Color color)
    {
        startVector.Normalize();
        endVector.Normalize();

        var angle = startVector.AngleOne(endVector);
        var angleStep = angle / (amount - (isEven ? 1 : 0));

        for (int i = 0; i < amount; i++)
        {
            Dust dust;

            if (isPerfect)
            {
                dust = SinglePerfect(type, position + startVector * offset, startVector * speed, scale, alpha,
                                              color);
            }
            else
            {
                dust = Single(type, position + startVector * offset, size, startVector * speed, scale, alpha,
                                       color);
            }

            yield return dust;

            startVector = startVector.RotatedBy(angleStep);
        }
    }

    public static List<Dust> Arc(int type, Vector2 position, Vector2 size, Vector2 startVector, Vector2 endVector,
                                 int amount, float speed, float scale = 1f, float offset = 0, int alpha = 0,
                                 Color color = default)
    {
        return GenerateArc(false, true, type, position, size, startVector, endVector, amount, speed, scale, offset,
                           alpha, color)
            .ToList();
    }

    public static List<Dust> ArcPerfect(int type, Vector2 position, Vector2 startVector, Vector2 endVector, int amount,
                                        float speed, float scale = 1f, float offset = 0, int alpha = 0,
                                        Color color = default)
    {
        return GenerateArc(true, true, type, position, Vector2.Zero, startVector, endVector, amount, speed, scale,
                           offset, alpha, color)
            .ToList();
    }

    public static List<Dust> Circle(int type, Vector2 position, Vector2 size, int amount, float speed, float scale = 1f,
                                    float offset = 0, int alpha = 0, Color color = default)
    {
        return GenerateArc(false, false, type, position, size, Vector2.UnitX, Vector2.UnitX, amount, speed, scale,
                           offset, alpha, color)
            .ToList();
    }

    public static List<Dust> CirclePerfect(int type, Vector2 position, int amount, float speed, float scale = 1f,
                                           float offset = 0, int alpha = 0, Color color = default)
    {
        return GenerateArc(true, false, type, position, Vector2.Zero, Vector2.UnitX, Vector2.UnitX, amount, speed,
                           scale, offset, alpha, color)
            .ToList();
    }
}
