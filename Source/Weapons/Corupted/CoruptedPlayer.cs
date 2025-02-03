using Terraria.DataStructures;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Weapons.Corupted;

public class CoruptedSource : WeaponWithAmmoSource
{
    public int SplitCount = 0;

    public CoruptedSource(WeaponWithAmmoSource source, int splitCount) : base(source)
    {
        SplitCount = splitCount;
    }
}