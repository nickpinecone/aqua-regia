using AquaRegia.Library;
using AquaRegia.Library.Extended.Fluent;
using AquaRegia.Library.Extended.Helpers;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Attributes;
using AquaRegia.Library.Extended.Modules.Projectiles;
using AquaRegia.Library.Extended.Modules.Shared;
using AquaRegia.Library.Extended.Players;
using AquaRegia.Library.Extended.Projectiles.Explosion;
using AquaRegia.Library.Tween;
using AquaRegia.Library.Tween.Ease;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.WoodenWater;

public enum TreeState
{
    Appear,
    Slam,
    Collide,
    End,
}

public class TreeProjectile : BaseProjectile
{
    public override string Texture => Assets.Sprites.Weapons.WoodenWater.TreeProjectile;

    private PropertyModule Property { get; } = new();

    [RuntimeModule] private StateModule<TreeState> State { get; } = new(TreeState.Appear);
    [RuntimeModule] private ImmunityModule Immunity { get; } = new();

    private Tween<int> Appear { get; } = Tween.Create<int>(6);
    private Tween<float> Rotate { get; } = Tween.Create<float>(20);
    private Tween<Vector2> Velocity { get; } = Tween.Create<Vector2>(20);

    private int _direction = 0;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Set(this)
            .Size(76, 66)
            .Damage(DamageClass.Ranged, -1)
            .Friendly(false, false)
            .TimeLeft(120)
            .DrawOffset(38)
            .Alpha(255);

        State.AddState(TreeState.Appear, HandleAppear);
        State.AddState(TreeState.Slam, HandleSlam);
    }

    private void HandleAppear()
    {
        Appear.Transition(255, 0)
            .OnTransition((value) => { Projectile.alpha = value; })
            .OnDone(() =>
            {
                Projectile.friendly = true;
                State.Current = TreeState.Slam;
            });
    }

    private void HandleSlam()
    {
        Rotate.Transition(MathHelper.ToRadians(-30 * _direction), MathHelper.ToRadians(110 * _direction))
            .OnTransition(Ease.InOut, (value) => { Projectile.rotation = value; });

        Velocity.Transition(Vector2.Zero, Vector2.UnitY.RotatedBy(MathHelper.ToRadians(-30 * _direction)) * 18f)
            .OnTransition(Ease.InOut, (value) => { Projectile.velocity = value; });

        if (Rotate.Done)
        {
            State.Current = TreeState.End;
            Projectile.Kill();
        }
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _direction = Main.rand.NextFromList(-1, 1);

        Projectile.rotation = MathHelper.ToRadians(-30 * _direction);
        Projectile.position -= new Vector2(90 * _direction, 160);

        if (TileArea.AnySolidInArea(Projectile.Center, 3, 3))
        {
            State.Current = TreeState.End;
            Projectile.Kill();
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (State.Current is TreeState.Slam)
        {
            State.Current = TreeState.Collide;

            Owner.GetModPlayer<ScreenShake>().Activate(6, 2);
            SoundEngine.PlaySound(SoundID.Item14);

            new ProjectileSpawner<ExplosionProjectile>()
                .Context(new ExplosionSource(this, Projectile.DamageType, 60, 100), Owner)
                .Position(Projectile.Center)
                .Damage(Projectile.damage, Projectile.knockBack)
                .Spawn();

            new DustSpawner(DustID.Cloud).Arc()
                .Position(Projectile.Bottom)
                .Size(new Vector2(2, 2), 5f)
                .Speed(9f, true)
                .Color(default, 75)
                .Edges(
                    Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-150)),
                    Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-30)),
                    6
                )
                .Spawn();
        }

        return base.OnTileCollide(oldVelocity);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        new DustSpawner(DustID.GrassBlades).Arc()
            .Position(Projectile.Center)
            .Size(new Vector2(12, 12))
            .Speed(2f)
            .Circle(6)
            .Spawn();

        if (State.Current != TreeState.Collide)
        {
            SoundEngine.PlaySound(SoundID.Grass);
        }
    }
}