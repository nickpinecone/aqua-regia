using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Terraria.ID;
using AquaRegia.Players;
using System;
using Terraria.Audio;

namespace AquaRegia.Weapons.Ice;

public class FrozenBomb : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Ice/FrozenBomb";

    public PropertyModule Property { get; private set; }
    public SpriteModule Sprite { get; private set; }
    public BounceModule Bounce { get; private set; }

    public Vector2 Size { get; private set; }
    public Rectangle WorldRectangle { get; private set; }

    private bool _colliding = false;

    public FrozenBomb() : base()
    {
        Property = new PropertyModule(this);
        Sprite = new SpriteModule(this);
        Bounce = new BounceModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this, 20, 20, 1, -1, 0, 0, 0, true, false, false);
        Property.SetTimeLeft(this, 240);
        Property.SetGravity(0.02f, 0.01f);

        Bounce.SetDefaults(null, -1);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.scale = 1.6f;
        Size = new Vector2(Projectile.width * (Projectile.scale + 0.6f), Projectile.height * (Projectile.scale + 0.6f));

        Main.LocalPlayer.GetModPlayer<IcePlayer>().AddBomb(this);
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        base.OnTileCollide(oldVelocity);

        _colliding = true;

        Projectile.velocity = Bounce.Update(this, oldVelocity, Projectile.velocity) ?? Projectile.velocity;
        Projectile.velocity.Y = Projectile.velocity.Y / 2f;

        if (_colliding && Projectile.velocity.Y <= 0f && Projectile.velocity.Y > -3f)
        {
            Projectile.velocity.Y = 0;
        }

        return false;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        SpawnProjectile<FrostExplosion>(Projectile.Center, Vector2.Zero, Projectile.damage, Projectile.knockBack);

        Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(6, 6);
        SoundEngine.PlaySound(SoundID.Item14);

        foreach (var particle in Particle.Circle(DustID.Ice, Projectile.Center, new Vector2(4, 4), 10, 3f))
        {
            particle.noGravity = false;
        }

        foreach (var particle in Particle.Circle(DustID.IceTorch, Projectile.Center, new Vector2(4, 4), 10, 5f))
        {
            particle.noGravity = false;
        }

        Main.LocalPlayer.GetModPlayer<IcePlayer>().RemoveBomb(this);
    }

    public void Explode()
    {
        Projectile.Kill();
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity.X *= 0.99f;
        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);

        if (_colliding && Projectile.velocity.Y >= 0f && Projectile.velocity.Y < 3f)
        {
            Projectile.velocity.Y = 0;
        }

        if (MathF.Abs(Projectile.velocity.X) > 0.2f)
        {
            Projectile.rotation += Sprite.RotateOnMove(Projectile.velocity, Math.Abs(Projectile.velocity.X / 32f));
        }

        _colliding = false;

        WorldRectangle = new Rectangle((int)(Projectile.Center.X - Size.X / 2), (int)(Projectile.Center.Y - Size.Y / 2),
                                       (int)Size.X, (int)Size.Y);
    }
}
