using WaterGuns.Sources;

namespace WaterGuns.Weapons.Corupted;

public class CoruptedSource : WeaponWithAmmoSource
{
    public int SplitCount = 0;

    public CoruptedSource(WeaponWithAmmoSource source, int splitCount) : base(source)
    {
        SplitCount = splitCount;
    }
}