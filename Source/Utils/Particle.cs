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
}
