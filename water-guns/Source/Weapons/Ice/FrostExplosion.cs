using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;
using Terraria.ID;

namespace WaterGuns.Weapons.Ice;

public class FrostExplosion : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }

    public FrostExplosion()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();
        Composite.AddModule(Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 128, 128, 1, -1, 0f, 0, 100, false);
        Property.SetTimeLeft(this, 15);
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        target.AddBuff(BuffID.Frozen, 240);
    }
}
