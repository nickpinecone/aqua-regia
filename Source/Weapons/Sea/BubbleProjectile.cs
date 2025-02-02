using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Sea;

public class BubbleProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Sea/BubbleProjectile";

    public PropertyModule Property { get; private set; }
    public HomeModule Home { get; private set; }
    public StickModule Stick { get; private set; }

    public Animation<int> Appear { get; private set; }
    public Animation<Vector2> Position { get; private set; }

    private SeaPlayer? _seaPlayer = null;
    private bool _wasConsumed = false;

    public BubbleProjectile() : base()
    {
        var immunity = new ImmunityModule();
        immunity.SetDefaults();
        Composite.AddRuntimeModule(immunity);

        Property = new PropertyModule();
        Home = new HomeModule();
        Stick = new StickModule();

        Composite.AddModule(Property, Home, Stick);

        Appear = new Animation<int>(10);
        Position = new Animation<Vector2>(20);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 20, 20, 1, -1, 0, 255);
        Property.SetTimeLeft(this, 120);
        Home.SetDefaults(speed: 2);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _seaPlayer = Owner.GetModPlayer<SeaPlayer>();

        Projectile.scale += Main.rand.NextFloat(-0.2f, 0.2f);
        Projectile.rotation = Main.rand.NextFloat(0f, MathHelper.TwoPi);
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (Stick.Target == null && !target.friendly && target.getRect().Intersects(Projectile.getRect()) &&
            _seaPlayer!.CanHome(target))
        {
            Projectile.friendly = false;

            Stick.ToTarget(target, Projectile.Center);

            Projectile.velocity = Vector2.Zero;
        }

        return base.CanHitNPC(target);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        if (!_wasConsumed)
        {
            Particle.Circle(DustID.BubbleBurst_Blue, Projectile.Center, new Vector2(8, 8), 4, 2f, 0.6f);
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.alpha = Appear.Animate(255, 100) ?? Projectile.alpha;

        if (Appear.Finished && Stick.Target == null)
        {
            Projectile.velocity =
                Home.Update(Projectile.Center, Projectile.velocity, (target) => _seaPlayer!.CanHome(target)) ??
                Projectile.velocity;
        }

        if (Stick.Target != null)
        {
            Projectile.Center = Position.Animate(Projectile.Center, Stick.Target.Center) ?? Projectile.Center;

            if (Position.Finished)
            {
                _wasConsumed = true;
                SoundEngine.PlaySound(SoundID.Item85);
                _seaPlayer!.AddBubble(Stick.Target);
                Projectile.Kill();
            }
        }
    }
}
