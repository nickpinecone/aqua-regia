using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Players;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Shotgun;

public class ChainProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Shotgun/ChainProjectile";

    public PropertyModule Property { get; private set; }
    public ChainModule Chain { get; private set; }
    public StickModule Stick { get; private set; }
    public BoomerangModule Boomerang { get; private set; }
    public HomeModule Home { get; private set; }

    public SoundStyle ChainHit { get; private set; }

    private ShotPlayer? _shotPlayer;
    private bool _didCollide;

    public ChainProjectile() : base()
    {
        Property = new PropertyModule();
        Chain = new ChainModule();
        Stick = new StickModule();
        Boomerang = new BoomerangModule();
        Home = new HomeModule();

        Composite.AddModule(Property, Chain, Stick, Boomerang, Home);

        ChainHit = new SoundStyle(AudioPath.Impact + "ChainHit") with
        {
            PitchVariance = 0.1f,
        };
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 16, 16, 1, -1);
        Property.SetTimeLeft(this, 120);
        Boomerang.SetDefaults(768f, 32f);

        Chain.SetTexture(TexturesPath.Weapons + "Shotgun/Chain", new Rectangle(0, 0, 6, 14));
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _shotPlayer = Owner.GetModPlayer<ShotPlayer>();

        Boomerang.SpawnPosition = Projectile.Center;
        Home.Speed = Projectile.velocity.Length();
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        _didCollide = true;

        return false;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        _shotPlayer!.Chain = null;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (!Boomerang.DidReachFar && !_didCollide)
        {
            _shotPlayer!.Chain = this;
            _shotPlayer.IsPulling = true;
            _shotPlayer.Target = target;

            Owner.GetModPlayer<ScreenShake>().Activate(6, 4);
            SoundEngine.PlaySound(ChainHit);

            Projectile.velocity = Vector2.Zero;
            Stick.ToTarget(target, Projectile.Center);
            Projectile.friendly = false;
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

        if (_didCollide || Boomerang.CheckFar(Projectile.Center))
        {
            Projectile.velocity =
                Home.Calculate(Projectile.Center, Projectile.velocity, Owner.Center);

            if (Boomerang.CheckReturn(Owner.Center, Projectile.Center))
            {
                Projectile.Kill();
            }
        }
        else if (_shotPlayer!.IsPulling)
        {
            Projectile.Center = (Vector2)Stick.HitPoint!;

            Owner.velocity =
                Home.Calculate(Projectile.Center, Projectile.velocity, Owner.Center)
                    .RotatedBy(MathHelper.Pi);

            if (Owner.Center.DistanceSQ(Stick.Target!.Center) < 32f * 32f ||
                Stick.Target.GetLifePercent() <= 0f)
            {
                _shotPlayer.IsPulling = false;
                Owner.velocity = Owner.velocity.RotatedBy(MathHelper.Pi) / 4f;
                Projectile.Kill();
            }
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);

        Chain.DrawChain(Owner.Center, Projectile.Center - velocity * 8f);

        return base.PreDraw(ref lightColor);
    }
}
