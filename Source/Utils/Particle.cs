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

    public static List<Dust> Arc(int type, Vector2 position, Vector2 startVector, Vector2 endVector, int amount,
                                 float speed, float scale = 1f, int alpha = 0, Color color = default)
    {
        var dustList = new List<Dust>();

        startVector.Normalize();
        endVector.Normalize();

        var angle = Helper.AngleBetween(startVector, endVector);
        var angleStep = angle / amount;

        for (int i = 0; i < amount; i++)
        {
            var dust = Particle.Single(type, position, new Vector2(1, 1), startVector * speed, scale, alpha, color);
            startVector = startVector.RotatedBy(angleStep);
            dustList.Add(dust);
        }

        return dustList;
    }

    public static List<Dust> ArcPerfect(int type, Vector2 position, Vector2 startVector, Vector2 endVector, int amount,
                                        float speed, float scale = 1f, int alpha = 0, Color color = default)
    {
        var dustList = new List<Dust>();

        startVector.Normalize();
        endVector.Normalize();

        var angle = Helper.AngleBetween(startVector, endVector);
        var angleStep = angle / (amount - 1);

        for (int i = 0; i < amount; i++)
        {
            var dust = Particle.SinglePerfect(type, position, startVector * speed, scale, alpha, color);
            startVector = startVector.RotatedBy(angleStep);
            dustList.Add(dust);
        }

        return dustList;
    }
}
