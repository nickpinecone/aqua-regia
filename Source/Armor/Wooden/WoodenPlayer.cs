using Terraria.DataStructures;
using Terraria.ModLoader;

namespace WaterGuns.Armor.Wooden;

public class WoodenSource : IEntitySource
{
    public int Direction;

    public string Context { get; set; }
    public WoodenSource(IEntitySource source)
    {
        Context = source.Context;
    }
}

public class WoodenPlayer : ModPlayer
{
    public RootProjectile Root = null;
}
