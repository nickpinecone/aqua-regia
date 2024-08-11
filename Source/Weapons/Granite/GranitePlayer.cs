using Microsoft.Xna.Framework;
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
    private bool _active = false;

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
