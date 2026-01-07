using AquaRegia.Library;
using AquaRegia.Library.Extended.Fluent;
using AquaRegia.Library.Extended.Helpers;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Projectiles;
using AquaRegia.Library.Tween;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace AquaRegia.Weapons.WoodenWater;

public class AcornProjectile : BaseProjectile
{
    public override string Texture => Assets.Sprites.Weapons.WoodenWater.acorn_projectile;

    private PropertyModule Property { get; } = new();

    [RuntimeModule] private ImmunityModule Immunity { get; } = new();
    [RuntimeModule] private HeadBounceModule HeadBounce { get; } = new();
    [RuntimeModule] private GravityModule Gravity { get; } = new();
    [RuntimeModule(1)] private RotateOnMoveModule RotateOnMove { get; } = new();

    private Tween<int> Appear { get; } = Tween.Create<int>(10);

    public override void SetDefaults()
    {
        base.SetDefaults();

        HeadBounce.OnHeadBounce = OnHeadBounce;

        Immunity.SetDefaults(10);
        RotateOnMove.SetDefaults();
        Gravity.SetDefaults();

        Property.Set(this)
            .Defaults.Ranged()
            .Size(20, 20)
            .Penetrate(5)
            .Alpha(255)
            .TimeLeft(120);
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.rotation = Main.rand.NextFloat(0f, MathHelper.TwoPi);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        new DustSpawner(DustExID.Wood).Single()
            .Position(Projectile.Center)
            .Size(new Vector2(10, 10))
            .Velocity(Vector2.Zero, false)
            .Spawn();
    }

    private void OnHeadBounce()
    {
        Gravity.Value /= 1.2f;
    }

    public override void AI()
    {
        base.AI();

        Appear.Transition(255, 0)
            .OnTransition((value) => { Projectile.alpha = value; });
    }
}