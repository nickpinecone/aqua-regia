using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Players;
using WaterGuns.Players.Weapons;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Sea;

public class HugeBubble : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "BubbleProjectile";

    public PropertyModule Property { get; private set; }
    public AnimationModule Animation { get; private set; }

    public NPC Target { get; set; }
    public Vector2 Size { get; private set; }
    public Rectangle WorldRectangle { get; private set; }

    public float MaxScale { get; private set; }
    public int MaxStage { get; private set; }
    public int Stage { get; private set; }

    public bool IsMaxStage
    {
        get {
            return Stage >= MaxStage;
        }
    }

    private SeaPlayer _seaPlayer = null;
    private bool _didDetach = false;
    private bool _didExplode = false;

    public HugeBubble() : base()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 600);

        Projectile.friendly = false;
        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.alpha = 200;

        MaxStage = 8;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        if (source is SeaSource seaSource)
        {
            Target = seaSource.Target;
        }

        var wideSide = Math.Max(Target.width, Target.height);
        MaxScale = (float)wideSide / Projectile.width + 0.2f;
        Size = new Vector2(Projectile.width * (MaxScale + 1), Projectile.height * (MaxScale + 1));

        _seaPlayer = Main.LocalPlayer.GetModPlayer<SeaPlayer>();
    }

    public void Enlarge()
    {
        if (!IsMaxStage)
        {
            Stage += 1;

            Projectile.timeLeft = Property.DefaultTime;

            if (IsMaxStage)
            {
                Projectile.friendly = true;
                Projectile.timeLeft = 300;
                Projectile.alpha = 155;
            }

            var scale = Animation.Animate<float>("scale", 0f, 0f, 10, Ease.Linear);
            scale.Start = MaxScale / MaxStage * Stage;
            scale.End = scale.Start + MaxScale / MaxStage;
            scale.Reset();
        }
    }

    public void Explode()
    {
        _didExplode = true;

        SoundEngine.PlaySound(SoundID.Item96);
        Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(6, 4);
        Particle.Circle(DustID.BubbleBurst_Blue, Projectile.Center, new Vector2(8, 8), 8, 4f, 1.5f);

        SpawnProjectile<BubbleExplosion>(Projectile.Center, Vector2.Zero, (int)(_seaPlayer.ProjectileDamage * 1.5f),
                                         1f);

        Projectile.Kill();
    }

    public override void OnKill(int timeLeft)
    {
        if (!_didExplode)
        {
            SoundEngine.PlaySound(SoundID.Item54);
            Particle.Circle(DustID.BubbleBurst_Blue, Projectile.Center, new Vector2(8, 8), 4, 2f, 0.8f);
        }

        if (!IsMaxStage && _didDetach)
        {
            for (int i = 0; i < Stage; i++)
            {
                var low = Projectile.scale * -4f;
                var high = Projectile.scale * 4f;

                var offset = new Vector2(Main.rand.NextFloat(low, high), Main.rand.NextFloat(low, high));

                SpawnProjectile<BubbleProjectile>(Projectile.Center + offset, Vector2.Zero, 1, 0);
            }
        }

        _seaPlayer.RemoveBubble(Target);
    }

    public override void AI()
    {
        base.AI();

        if (Target != null)
        {
            if (!IsMaxStage || Target.boss)
            {
                Projectile.Center = Target.Center;
            }
            else
            {
                var x = MathF.Sin(Projectile.Center.Y / 12f);

                Projectile.velocity = new Vector2(x, -1);
                Target.velocity = new Vector2(0, -1);
                Target.Center = Projectile.Center;
            }

            if (Target.GetLifePercent() <= 0f)
            {
                _didDetach = true;
                Projectile.Kill();
            }
        }

        var scale = Animation.Get<float>("scale");
        Projectile.scale = scale?.Update() ?? Projectile.scale;

        WorldRectangle = new Rectangle((int)(Projectile.Center.X - Size.X / 2), (int)(Projectile.Center.Y - Size.Y / 2),
                                       (int)Size.X, (int)Size.Y);
    }
}
