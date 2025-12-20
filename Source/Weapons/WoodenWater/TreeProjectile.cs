using AquaRegia.Library;
using AquaRegia.Library.Extended.Helpers;
using AquaRegia.Library.Extended.Modules;
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
    public override string Texture => Assets.Weapons + $"{nameof(WoodenWater)}/{nameof(TreeProjectile)}";

    private PropertyModule Property { get; }
    private StateModule<TreeState> State { get; }

    private Tween<int> Appear { get; }
    private Tween<float> Rotate { get; }
    private Tween<Vector2> Velocity { get; }

    private int _direction = 0;

    public TreeProjectile()
    {
        Property = new PropertyModule(this);
        State = new StateModule<TreeState>(TreeState.Appear);

        Composite.AddModule(Property, State);
        Composite.AddRuntimeModule(new ImmunityModule(), State);

        Appear = Tween.Create<int>(6);
        Rotate = Tween.Create<float>(20);
        Velocity = Tween.Create<Vector2>(20);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Size(76, 66)
            .Damage(DamageClass.Ranged, -1)
            .Friendly(false, false)
            .TimeLeft(120)
            .Alpha(255);

        Projectile.gfxOffY = 38;

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

        if (TileHelper.AnySolidInArea(Projectile.Center, 3, 3))
        {
            Projectile.Kill();
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (State.Current is TreeState.Slam or TreeState.Appear)
        {
            State.Current = TreeState.Collide;

            Owner.GetModPlayer<ScreenShake>().Activate(6, 2);
            SoundEngine.PlaySound(SoundID.Item14);

            ModHelper.SpawnExplosion(
                new ExplosionSource(this, Projectile.DamageType, 100, Projectile.damage, Projectile.knockBack, 100),
                Owner, Projectile.Center
            );

            foreach (var particle in DustHelper.Arc(DustID.Cloud, Projectile.Bottom, new Vector2(2, 2),
                         Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-150)),
                         Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-30)), 6, 9f, 5f, 0, 75))
            {
                particle.noGravity = true;
            }
        }

        return base.OnTileCollide(oldVelocity);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        DustHelper.Circle(DustID.GrassBlades, Projectile.Center, new Vector2(12, 12), 6, 2f, 1f);

        if (State.Current != TreeState.Collide)
        {
            SoundEngine.PlaySound(SoundID.Grass);
        }
    }
}