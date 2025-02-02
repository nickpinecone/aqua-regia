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

    private Timer _explodeDelay;

    public FrozenBomb() : base()
    {
        Property = new PropertyModule();
        Sprite = new SpriteModule();

        Composite.AddModule(Property, Sprite);

        _explodeDelay = new Timer(0);
        _explodeDelay.Paused = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 20, 20, 1, -1, 0, 0, 0, true, false, false);
        Property.SetTimeLeft(this, 240);

        DrawOriginOffsetY = -8;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Owner.GetModPlayer<IcePlayer>().Bombs.Add(this);
        Projectile.rotation = Main.rand.NextFloat(0, MathHelper.Pi);

        if (source is IceSource iceSource)
        {
            _explodeDelay.WaitTime = iceSource.ExplodeDelay;
            Projectile.timeLeft += iceSource.ExplodeDelay;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        base.OnTileCollide(oldVelocity);

        return false;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Helper.SpawnProjectile<FrostExplosion>(Projectile.GetSource_FromThis(), Owner, Projectile.Center, Vector2.Zero, Projectile.damage, Projectile.knockBack);

        Owner.GetModPlayer<ScreenShake>().Activate(4, 8);
        SoundEngine.PlaySound(SoundID.Item14);

        foreach (var particle in Particle.Circle(DustID.Ice, Projectile.Center, new Vector2(4, 4), 10, 3f))
        {
            particle.noGravity = false;
        }

        foreach (var particle in Particle.Circle(DustID.IceTorch, Projectile.Center, new Vector2(4, 4), 10, 5f))
        {
            particle.noGravity = false;
        }

        if (!_explodeDelay.Done)
        {
            Owner.GetModPlayer<IcePlayer>().Bombs.Remove(this);
        }
    }

    public void Explode()
    {
        _explodeDelay.Restart();
    }

    public override bool PreAI()
    {
        base.PreAI();

        _explodeDelay.Update();

        if (_explodeDelay.Done)
        {
            Projectile.Kill();
        }

        return !_explodeDelay.Done;
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity *= 0.98f;

        if (MathF.Abs(Projectile.velocity.X) > 0.2f)
        {
            Projectile.rotation += Sprite.RotateOnMove(Projectile.velocity, Math.Abs(Projectile.velocity.X / 32f));
        }

        var top = Projectile.Center + new Vector2(4, -18).RotatedBy(Projectile.rotation);
        var particle = Particle.SinglePerfect(DustID.IceTorch, top, Main.rand.NextVector2Unit());
        particle.noGravity = true;
    }
}
