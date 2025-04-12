using System.CommandLine.Help;
using System.Formats.Tar;
using WaterGuns.Utils;
using Stubble.Core.Imported;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace WaterGuns.Weapons.Granite;

public class GraniteSource : IEntitySource
{
    public NPC Target;

    public string? Context { get; private set; }
    public GraniteSource(IEntitySource source, NPC target)
    {
        Context = source.Context;
        Target = target;
    }
}

public class GranitePlayer : ModPlayer
{
    private bool _active = false;

    public bool IsActive()
    {
        return _active;
    }

    public void Activate(int damage)
    {
        _active = true;

        var direction = Main.MouseWorld - Player.Center;
        direction.Normalize();
        var velocity = direction * 24f;

        Helper.SpawnProjectile<GraniteElemental>(Projectile.GetSource_NaturalSpawn(), Player, Player.Center, velocity, damage, 0);
    }

    public void Deactivate()
    {
        _active = false;
    }

    public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
    {
        return !_active;
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        base.ModifyDrawInfo(ref drawInfo);

        if (_active)
        {
            drawInfo.drawPlayer.opacityForAnimation = 0;
        }
        else
        {
            drawInfo.drawPlayer.opacityForAnimation = 1f;
        }
    }
}
