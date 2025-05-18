using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Players;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Sea;

public class HugeBubble : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Sea/BubbleProjectile";

    public PropertyModule Property { get; private set; }
    public Animation<float> Scale { get; private set; }

    public NPC? Target { get; set; }
    public Vector2 Size { get; private set; }
    public Rectangle WorldRectangle { get; private set; }

    public float MaxScale { get; private set; }
    public int MaxStage { get; private set; }
    public int Stage { get; private set; }

    public bool IsMaxStage
    {
        get
        {
            return Stage >= MaxStage;
        }
    }

    private SeaPlayer? _seaPlayer = null;
    private bool _didDetach = false;
    private bool _didExplode = false;

    public HugeBubble() : base()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();

        Composite.AddModule(Property);

        Scale = new Animation<float>(10);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 20, 20, 1, -1, 0, 200, 0, true, false, false);
        Property.SetTimeLeft(this, 600);

        MaxStage = 8;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        if (source is SeaSource seaSource)
        {
            Target = seaSource.Target;
        }

        var wideSide = Math.Max(Target!.width, Target.height);
        MaxScale = (float)wideSide / Projectile.width + 0.2f;
        Size = new Vector2(Projectile.width * (MaxScale + 1), Projectile.height * (MaxScale + 1));

        _seaPlayer = Owner.GetModPlayer<SeaPlayer>();
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

            Scale.Start = MaxScale / MaxStage * Stage;
            Scale.End = Scale.Start + MaxScale / MaxStage;
            Scale.Reset();
            Scale.Initiate = false;
        }
    }

    public void Explode()
    {
        _didExplode = true;

        SoundEngine.PlaySound(SoundID.Item96);
        Owner.GetModPlayer<ScreenShake>().Activate(6, 4);
        Particle.Circle(DustID.BubbleBurst_Blue, Projectile.Center, new Vector2(8, 8), 8, 4f, 1.5f);

        Helper.SpawnProjectile<BubbleExplosion>(Projectile.GetSource_FromThis(), Owner, Projectile.Center, Vector2.Zero,
                                                (int)(_seaPlayer!.ProjectileDamage * 1.5f), 1f);

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

                Helper.SpawnProjectile<BubbleProjectile>(Projectile.GetSource_FromThis(), Owner,
                                                         Projectile.Center + offset, Vector2.Zero, 1, 0);
            }
        }

        _seaPlayer!.RemoveBubble(Target!);
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

        Projectile.scale = Scale.Animate(Scale.Start, Scale.End) ?? Projectile.scale;

        WorldRectangle = new Rectangle((int)(Projectile.Center.X - Size.X / 2), (int)(Projectile.Center.Y - Size.Y / 2),
                                       (int)Size.X, (int)Size.Y);
    }
}
