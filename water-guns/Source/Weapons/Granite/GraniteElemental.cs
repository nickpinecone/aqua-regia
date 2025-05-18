using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Granite;

public class GraniteElemental : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Granite/GraniteElemental";

    public PropertyModule Property { get; private set; }
    public SpriteModule Sprite { get; private set; }

    public SoundStyle RockCrush { get; private set; }

    public GraniteElemental()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();
        Sprite = new SpriteModule();

        Composite.AddModule(Property, Sprite);

        RockCrush = new SoundStyle(AudioPath.Kill + "RockCrush") with
        {
            Volume = 0.6f,
            PitchVariance = 0.1f,
        };
    }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.projFrames[Projectile.type] = 7;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 30, 30, 1, -1);
        Property.SetTimeLeft(this, 20);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        SoundEngine.PlaySound(RockCrush);
        Particle.Circle(DustID.Granite, Projectile.Center, new Vector2(12, 12), 6, 3f, 0.8f);

        Owner.GetModPlayer<GranitePlayer>().Deactivate();
        Owner.velocity = Projectile.velocity / 3f;
    }

    public override void AI()
    {
        base.AI();

        Owner.Center = Projectile.Center;
        Projectile.rotation += Sprite.RotateOnMove(Projectile.velocity, 0.2f);
        Sprite.Animate(this, 4);
    }
}
