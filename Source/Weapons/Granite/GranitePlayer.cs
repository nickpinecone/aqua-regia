using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace WaterGuns.Weapons.Granite;

public class GraniteSource : IEntitySource
{
    public NPC Target;

    public string Context { get; set; }
    public GraniteSource(IEntitySource source)
    {
        Context = source.Context;
    }
}

public class GranitePlayer : ModPlayer
{
}
