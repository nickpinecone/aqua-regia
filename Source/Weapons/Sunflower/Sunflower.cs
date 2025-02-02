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
    public Animation<int> Appear { get; private set; }

    public Timer SeedTimer { get; private set; }
    public Timer VineTimer { get; private set; }
    public Timer ParticleTimer { get; private set; }
    public Vector2 Offset { get; private set; } = new Vector2(0, 48);

    private Dictionary<Dust, Vector2> _particles = new();
    private List<Dust> _removeQueue = new();

    public Sunflower() : base()
    {
        Property = new PropertyModule();
        Composite.AddModule(Property);

        Appear = new Animation<int>(10);

        ParticleTimer = new Timer(10);
        SeedTimer = new Timer(20);
        VineTimer = new Timer(60);
    }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.projFrames[Projectile.type] = 2;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 30, 66, 0, -1, 0, 0, 0, false, false, false);
        Property.SetTimeLeft(this, 11);

        Projectile.hide = true;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Main.LocalPlayer.GetModPlayer<SunflowerPlayer>().Sunflower = this;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Main.LocalPlayer.GetModPlayer<SunflowerPlayer>().Sunflower = null;
    }

    public override void AI()
    {
        base.AI();

        Projectile.frame = Main.IsItDay() ? 0 : 1;

        if (Projectile.timeLeft <= 10)
        {
            Projectile.alpha = Appear.Backwards() ?? Projectile.alpha;
        }
        else
        {
            Projectile.alpha = Appear.Animate(255, 0) ?? Projectile.alpha;
        }

        if (!Main.IsItDay())
        {
            AttackAtNight();
        }
        SpawnParticles();
    }

    public void AttackAtNight()
    {
        SeedTimer.Update();

        if (SeedTimer.Done)
        {
            SeedTimer.Restart();

            Helper.SpawnProjectile<SeedProjectile>(Projectile.GetSource_FromThis(), Owner, Projectile.Top, Vector2.One, Projectile.damage, Projectile.knockBack);
        }

        if (Main.LocalPlayer.GetModPlayer<SunflowerPlayer>().BloodVine == null)
        {
            VineTimer.Update();

            if (VineTimer.Done)
            {
                VineTimer.Restart();

                var target = Helper.FindNearestNPC(Projectile.Center, 1000f);

                if (target != null)
                {
                    Helper.SpawnProjectile<BloodVine>(Projectile.GetSource_FromThis(), Owner, Owner.Top, Vector2.Zero, Projectile.damage / 2, 0);
                }
            }
        }
    }

    public void SpawnParticles()
    {
        ParticleTimer.Update();

        var particleType = Main.IsItDay() ? DustID.YellowTorch : DustID.Blood;

        if (ParticleTimer.Done)
        {
            ParticleTimer.Restart();

            var offset = new Vector2(0, -1).RotatedByRandom(MathHelper.PiOver2) * 24f;
            var position = Projectile.Top + offset + new Vector2(0, 8);
            var velocity = offset.RotatedBy(MathHelper.Pi);
            velocity.Normalize();
            velocity *= 2f;

            var particle = Particle.SinglePerfect(particleType, position, velocity, 1.6f);
            particle.noGravity = true;
            particle.fadeIn = 1f;

            _particles[particle] = Projectile.Center;
        }

        foreach (var particle in _particles.Keys)
        {
            if (!particle.active || particle.type != particleType)
            {
                _removeQueue.Add(particle);
            }
            else
            {
                var diff = Projectile.Center - _particles[particle];
                particle.position += diff;

                _particles[particle] = Projectile.Center;
            }
        }

        foreach (var particle in _removeQueue)
        {
            _particles.Remove(particle);
        }
        _removeQueue.Clear();
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs,
                                    List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        overPlayers.Add(index);
    }
}
