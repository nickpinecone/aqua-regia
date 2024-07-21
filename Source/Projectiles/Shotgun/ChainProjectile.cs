using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using WaterGuns.Players;
using WaterGuns.Players.Weapons;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Shotgun;

public class ChainProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public ChainModule Chain { get; private set; }
    public StickModule Stick { get; private set; }
    public SoundStyle ChainHit { get; private set; }

    private ShotPlayer _shotPlayer;
    private bool _didCollide;

    public ChainProjectile() : base()
    {
        Property = new PropertyModule(this);
        Chain = new ChainModule(this);
        Stick = new StickModule(this);

        ChainHit = new SoundStyle(AudioPath.Impact + "ChainHit") with {
            PitchVariance = 0.1f,
        };
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 120);

        Chain.SetTexture(TexturesPath.Projectiles + "ChainProjectile", new Rectangle(0, 0, 6, 14));
        Chain.MaxPosition = 768f;
        Chain.PlayerClose = 32f;

        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _shotPlayer = Main.LocalPlayer.GetModPlayer<ShotPlayer>();

        Chain.SpawnPosition = Projectile.Center;
        Chain.BackSpeed = Projectile.velocity.Length();
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        _didCollide = true;

        return false;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        _shotPlayer.Chain = null;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (!Chain.IsFarAway && !_didCollide)
        {
            _shotPlayer.Chain = this;
            _shotPlayer.IsPulling = true;
            _shotPlayer.Target = target;

            Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(6, 4);
            SoundEngine.PlaySound(ChainHit);

            Projectile.velocity = Vector2.Zero;
            Stick.ToTarget(target, Projectile.Center);
            Projectile.friendly = false;
        }
    }

    public override void AI()
    {
        base.AI();

        if (_didCollide || Chain.IsFarAway)
        {
            Projectile.velocity = Chain.ReturnToPlayer(Main.LocalPlayer.Center, Projectile.Center, Projectile.velocity, 1);

            if (Chain.Returned)
            {
                Projectile.Kill();
            }
        }
        else if (Stick.Target == null)
        {
            Chain.Update(Projectile.Center);
        }
        else if (_shotPlayer.IsPulling)
        {
            Projectile.Center = Stick.HitPoint;

            Main.LocalPlayer.velocity =
                Chain.ReturnToPlayer(Main.LocalPlayer.Center, Projectile.Center, Projectile.velocity).RotatedBy(MathHelper.Pi);

            if (Main.LocalPlayer.Center.DistanceSQ(Stick.Target.Center) < 32f * 32f ||
                Stick.Target.GetLifePercent() <= 0f)
            {
                _shotPlayer.IsPulling = false;
                Main.LocalPlayer.velocity = Main.LocalPlayer.velocity.RotatedBy(MathHelper.Pi) / 4f;
                Projectile.Kill();
            }
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Chain.DrawChain(Main.LocalPlayer.Center, Projectile.Center);

        return base.PreDraw(ref lightColor);
    }
}
