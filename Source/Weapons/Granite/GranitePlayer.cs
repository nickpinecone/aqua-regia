using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Weapons.Granite;

public class GraniteSource : ProjectileSource
{
    public NPC Target;

    public GraniteSource(IEntitySource source) : base(source)
    {
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

        var direction = Main.MouseWorld - Main.LocalPlayer.Center;
        direction.Normalize();
        var velocity = direction * 24f;

        Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), Main.LocalPlayer.Center, velocity,
                                 ModContent.ProjectileType<GraniteElemental>(), damage, 0, Main.LocalPlayer.whoAmI);
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
