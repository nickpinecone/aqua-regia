using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Players;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Sea;

public class BubbleProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "BubbleProjectile";

    public AnimationModule Animation { get; private set; }
    public PropertyModule Property { get; private set; }
    public HomeModule Home { get; private set; }
    public StickModule Stick { get; private set; }

    private SeaPlayer _seaPlayer = null;

    public BubbleProjectile() : base()
    {
        Animation = new AnimationModule(this);
        Property = new PropertyModule(this);
        Home = new HomeModule(this);
        Stick = new StickModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 120);
        Home.SetDefaults();

        Home.Speed = 2;

        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.alpha = 255;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _seaPlayer = Main.LocalPlayer.GetModPlayer<SeaPlayer>();

        Projectile.scale += Main.rand.NextFloat(-0.2f, 0.2f);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (_seaPlayer.CanHome(target))
        {
            Stick.ToTarget(target, Projectile.Center);

            Projectile.friendly = false;
            Projectile.velocity = Vector2.Zero;
        }
    }

    public override void AI()
    {
        base.AI();

        var appear = Animation.Animate<int>("appear", 255, 100, 10, Ease.Linear);
        Projectile.alpha = appear.Value ?? Projectile.alpha;

        if (appear.Finished && Stick.Target == null)
        {
            Projectile.velocity =
                Home.Update(Projectile.Center, Projectile.velocity, (target) => _seaPlayer.CanHome(target)) ??
                Projectile.velocity;
        }

        if (Stick.Target != null)
        {
            var pos = Animation.Animate<Vector2>("pos", Projectile.Center, Stick.Target.Center, 20, Ease.Linear);
            Projectile.Center = pos.Value ?? Projectile.Center;

            if (pos.Finished)
            {
                SoundEngine.PlaySound(SoundID.Item85);
                _seaPlayer.Enlarge(Stick.Target);
                Projectile.Kill();
            }
        }
    }
}
