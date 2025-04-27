using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Players;
using WaterGuns.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace WaterGuns.Weapons.Sunflower;

public class SeedExplosion : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }

    public SeedExplosion()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();
        Composite.AddModule(Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 32, 32, 1, -1, 0, 0, 100, false);
        Property.SetTimeLeft(this, 15);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Owner.GetModPlayer<ScreenShake>().Activate(2, 2);
        SoundEngine.PlaySound(SoundID.Item14);

        foreach (var particle in Particle.Circle(DustID.Smoke, Projectile.Center, new Vector2(2, 2), 6, 1f, 1.5f))
        {
            particle.noGravity = true;
        }

        foreach (var particle in Particle.Circle(DustID.Torch, Projectile.Center, new Vector2(2, 2), 6, 2f, 2f))
        {
            particle.noGravity = true;
        }
    }
}
