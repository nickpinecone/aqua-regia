using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Utils;

// DustID does not have these
public static class ParticleID
{
    public static short Wood = 7;
}

public static class Particle
{
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

    private static IEnumerable<Vector2> GenerateArc(Vector2 startVector, Vector2 endVector, int amount)
    {
        startVector.Normalize();
        endVector.Normalize();

        var angle = Helper.AngleOne(startVector, endVector);
        var angleStep = angle / (amount - 1);

        for (int i = 0; i < amount; i++)
        {
            yield return startVector;

            startVector = startVector.RotatedBy(angleStep);
        }
    }

    public static List<Dust> Arc(int type, Vector2 position, Vector2 size, Vector2 startVector, Vector2 endVector,
                                 int amount, float speed, float scale = 1f, float offset = 0, int alpha = 0,
                                 Color color = default)
    {
        var dustList = new List<Dust>();

        foreach (var velocity in GenerateArc(startVector, endVector, amount))
        {
            var dust = Particle.Single(type, position + velocity * offset, size, velocity * speed, scale, alpha, color);
            dustList.Add(dust);
        }

        return dustList;
    }

    public static List<Dust> ArcPerfect(int type, Vector2 position, Vector2 startVector, Vector2 endVector, int amount,
                                        float speed, float scale = 1f, float offset = 0, int alpha = 0,
                                        Color color = default)
    {
        var dustList = new List<Dust>();

        foreach (var velocity in GenerateArc(startVector, endVector, amount))
        {
            var dust =
                Particle.SinglePerfect(type, position + velocity * offset, velocity * speed, scale, alpha, color);
            dustList.Add(dust);
        }

        return dustList;
    }

    public static List<Dust> Circle(int type, Vector2 position, Vector2 size, int amount, float speed, float scale = 1f,
                                    float offset = 0, int alpha = 0, Color color = default)
    {
        return Arc(type, position, size, Vector2.UnitX, Vector2.UnitX, amount, speed, scale, offset, alpha, color);
    }

    public static List<Dust> CirclePerfect(int type, Vector2 position, int amount, float speed, float scale = 1f,
                                           float offset = 0, int alpha = 0, Color color = default)
    {
        return ArcPerfect(type, position, Vector2.UnitX, Vector2.UnitX, amount, speed, scale, offset, alpha, color);
    }
}
