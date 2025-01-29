using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Players;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Sea;

public class StarfishProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Sea/StarfishProjectile";

    public HomeModule Home { get; private set; }
    public PropertyModule Property { get; private set; }
    public StickModule Stick { get; private set; }
    public SpriteModule Sprite { get; private set; }
    public BoomerangModule Boomerang { get; private set; }
    public ImmunityModule Immunity { get; private set; }

    public Timer StickTimer { get; private set; }
    public Animation<Vector2> Reverse { get; private set; }

    private Vector2 _beforeHit = Vector2.Zero;
    private bool _reachedFar = false;

    public StarfishProjectile() : base()
    {
        Property = new PropertyModule();
        Stick = new StickModule();
        Home = new HomeModule();
        Sprite = new SpriteModule();
        Boomerang = new BoomerangModule();
        Immunity = new ImmunityModule();

        Composite.AddModule(Property, Stick, Home, Sprite, Boomerang);

        StickTimer = new Timer(10);
        Reverse = new Animation<Vector2>(20, Ease.InOut);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Immunity.SetDefaults(10);
        Property.SetProperties(this, 20, 18, 1, -1);
        Property.SetTimeLeft(this, 600);
        Boomerang.SetDefaults(512f);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Boomerang.SpawnPosition = Projectile.Center;
    }

    public override bool? CanHitNPC(NPC target)
    {
        return Immunity.CanHit(target) ? null : false;
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        SoundEngine.PlaySound(SoundID.NPCHit9);

        if (Stick.Target == null)
        {
            Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(4, 2);

            Stick.ToTarget(target, Projectile.Center);

            _beforeHit = Projectile.velocity;
            Projectile.velocity.Normalize();
            StickTimer.Restart();
        }

        var invert = Projectile.velocity.RotatedBy(MathHelper.Pi);
        var start = invert.RotatedBy(-MathHelper.PiOver4);
        var end = invert.RotatedBy(MathHelper.PiOver4);
        Particle.Arc(DustID.Blood, (Vector2)Stick.HitPoint!, new Vector2(8, 8), start, end, 6, 2f, 1.2f);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        var particle = Particle.Single(DustID.Coralstone, Projectile.Center, new Vector2(8, 8), Vector2.Zero, 1f);
        particle.noGravity = true;
    }

    public override bool PreAI()
    {
        StickTimer.Update();
        Immunity.Update();

        return true;
    }

    public override void AI()
    {
        base.AI();

        if (_reachedFar)
        {
            Projectile.velocity = Reverse.Animate(Projectile.velocity, Projectile.velocity.RotatedBy(MathHelper.Pi)) ??
                                  Projectile.velocity;

            if (Reverse.Finished)
            {
                Projectile.velocity = Home.Calculate(Projectile.Center, Projectile.velocity, Owner.Center);
            }

            if (Boomerang.IsReturned(Owner.Center, Projectile.Center))
            {
                Projectile.Kill();
            }
        }
        else
        {
            var isFar = Boomerang.IsFar(Projectile.Center);

            if (isFar && Stick.Target == null)
            {
                _reachedFar = true;
            }
        }

        Projectile.rotation += Sprite.RotateOnMove(Projectile.velocity, 0.2f);

        if (StickTimer.Done && Stick.Target != null)
        {
            Stick.Detach();
            Projectile.velocity = _beforeHit;
        }
    }
}
