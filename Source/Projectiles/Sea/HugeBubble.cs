using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WaterGuns.Players;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Sea;

public class HugeBubble : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "BubbleProjectile";

    public PropertyModule Property { get; private set; }
    public AnimationModule Animation { get; private set; }

    public NPC Target { get; set; }

    public float MaxScale { get; private set; }
    public int MaxSize { get; private set; }
    public int Size { get; private set; }

    public bool IsMaxSize
    {
        get {
            return Size >= MaxSize;
        }
    }

    private Animation<float> _scaleAnimation = null;
    private SeaPlayer _seaPlayer = null;

    public HugeBubble() : base()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
    }

    public void Enlarge()
    {
        if (!IsMaxSize)
        {
            Size += 1;

            if (IsMaxSize)
            {
                Projectile.friendly = true;
                Projectile.timeLeft = 120;
            }

            _scaleAnimation = Animation.Animate<float>("scale", Projectile.scale, Projectile.scale + MaxScale / MaxSize,
                                                       10, Ease.Linear);

            _scaleAnimation.Start = MaxScale / MaxSize * Size;
            _scaleAnimation.End = _scaleAnimation.Start + MaxScale / MaxSize;
            _scaleAnimation.Reset();
        }
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 2);

        Projectile.friendly = false;
        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.alpha = 100;

        MaxSize = 8;
        MaxScale = 3f;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _seaPlayer = Main.LocalPlayer.GetModPlayer<SeaPlayer>();
    }

    public override void OnKill(int timeLeft)
    {
        if (!IsMaxSize)
        {
            for (int i = 0; i < Size; i++)
            {
                SpawnProjectile<BubbleProjectile>(Projectile.Center, Vector2.Zero, 1, 0);
            }
        }

        _seaPlayer.Remove(Target);
    }

    public override void AI()
    {
        base.AI();

        if (Target != null)
        {
            if (!IsMaxSize)
            {
                Projectile.Center = Target.Center;
                Projectile.timeLeft = 2;
            }
            else
            {
                Projectile.velocity = new Vector2(0, -1);
                Target.velocity = new Vector2(0, -1);
                Target.Center = Projectile.Center;
            }

            if (Target.GetLifePercent() <= 0f)
            {
                Projectile.Kill();
            }
        }

        _scaleAnimation = Animation.Animate<float>("scale", 0f, MaxScale / MaxSize, 10, Ease.Linear);

        Projectile.scale = _scaleAnimation.Value ?? Projectile.scale;
    }
}
