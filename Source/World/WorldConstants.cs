using Terraria;

namespace AquaRegia.World;

public static class WorldConstants
{
    // TODO Rename to sea level
    public static double FloodLevel => Main.worldSurface - (Main.worldSurface * 0.5f);
}