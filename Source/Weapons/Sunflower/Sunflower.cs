using System.Collections.Generic;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AquaRegia.Weapons.Sunflower;

public class Sunflower : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Sunflower/Sunflower";

    public PropertyModule Property { get; private set; }
    public AnimationModule Animation { get; private set; }
    public Timer ParticleTimer { get; private set; }

    private Dictionary<Dust, Vector2> _particles = new();
    private List<Dust> _removeQueue = new();

    public Sunflower() : base()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
        ParticleTimer = new Timer(10, this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 2);

        Projectile.damage = 0;
        Projectile.penetrate = -1;

        Projectile.width = 30;
        Projectile.height = 66;

        Projectile.hostile = false;
        Projectile.friendly = false;
        Projectile.tileCollide = false;
    }

    public override void AI()
    {
        base.AI();

        Projectile.timeLeft = Property.DefaultTime;
        Projectile.Center = Helper.ToVector2I(Main.LocalPlayer.Center);

        var offset = new Vector2(0, 10);
        var slide = Animation.Animate<Vector2>("slide", Vector2.Zero + offset,
                                               new Vector2(0, Projectile.Size.Y / 2) + offset, 60, Ease.Linear);
        Projectile.Center -= Helper.ToVector2I(slide.Update() ?? slide.End);

        SpawnSunParticles();
    }

    public void SpawnSunParticles()
    {
        if (ParticleTimer.Done)
        {
            ParticleTimer.Restart();

            var offset = new Vector2(0, -1).RotatedByRandom(MathHelper.PiOver2) * 24f;
            var position = Projectile.Top + offset + new Vector2(0, 8);
            var velocity = offset.RotatedBy(MathHelper.Pi);
            velocity.Normalize();
            velocity *= 2f;

            var particle = Particle.SinglePerfect(DustID.YellowTorch, position, velocity, 1.6f);
            particle.noGravity = true;
            particle.fadeIn = 1f;

            _particles[particle] = Main.screenPosition;
        }

        foreach (var particle in _particles.Keys)
        {
            if (!particle.active || particle.type != DustID.YellowTorch)
            {
                _removeQueue.Add(particle);
            }
            else
            {
                var diff = Main.screenPosition - _particles[particle];
                particle.position += diff;

                _particles[particle] = Main.screenPosition;
            }
        }

        foreach (var particle in _removeQueue)
        {
            _particles.Remove(particle);
        }
        _removeQueue.Clear();
    }
}
