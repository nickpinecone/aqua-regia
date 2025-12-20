using Terraria.Utilities;

namespace AquaRegia.Library.Extended.Extensions;

public static class RandomExtensions
{
    public static bool Percent(this UnifiedRandom rand, int percent)
    {
        return rand.Next(0, 100) < percent;
    }
}