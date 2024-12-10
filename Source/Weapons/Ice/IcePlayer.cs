using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Weapons.Ice;

public class IceSource : ProjectileSource
{
    public bool IsBombExploder;

    public IceSource(IEntitySource source) : base(source)
    {
    }
}

public class IcePlayer : ModPlayer
{
    public FrozenBomb Bomb { get; set; }
    public bool HasExploder { get; set; }

    public bool ListenForRelease { get; set; }
    public bool ReleasedRight { get; set; }

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
