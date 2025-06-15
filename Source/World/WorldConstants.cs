using Terraria;

namespace AquaRegia.World;

public static class WorldConstants
{
    public static double FloodLevel => Main.worldSurface - (Main.worldSurface * 0.5f);
}