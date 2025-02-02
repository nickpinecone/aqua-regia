using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Weapons.Ice;

public class IceSource : IEntitySource
{
    public int ExplodeDelay = 0;

    public string? Context { get; private set; }
    public IceSource(IEntitySource source, int delay)
    {
        Context = source.Context;
        ExplodeDelay = delay;
    }
}

public class IcePlayer : ModPlayer
{
    public List<FrozenBomb> Bombs { get; set; } = new();

    public bool ListenForRelease { get; set; }
    public bool ReleasedRight { get; set; } = true;

    public override void PreUpdate()
    {
        base.PreUpdate();

        if (ListenForRelease && !Main.mouseRight)
        {
            ListenForRelease = false;
            ReleasedRight = true;
        }
    }
}
