using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Granite;

public class GraniteElemental : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Granite/GraniteElemental";

    public PropertyModule Property { get; private set; }
    public AnimationModule Animation { get; private set; }
    public SpriteModule Sprite { get; private set; }

    public SoundStyle RockCrush { get; private set; }

    public GraniteElemental()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
        Sprite = new SpriteModule(this);

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

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 20);

        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.tileCollide = true;
        Projectile.width = 30;
        Projectile.height = 30;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        SoundEngine.PlaySound(RockCrush);
        Particle.Circle(DustID.Granite, Projectile.Center, new Vector2(12, 12), 6, 3f, 0.8f);

        Main.LocalPlayer.GetModPlayer<GranitePlayer>().Deactivate();
        Main.LocalPlayer.velocity = Projectile.velocity / 3f;
    }

    public override void AI()
    {
        base.AI();

        Main.LocalPlayer.Center = Projectile.Center;
        Projectile.rotation += Sprite.RotateOnMove(Projectile.velocity, 0.2f);
        Animation.Sprite(this, 4);
    }
}
