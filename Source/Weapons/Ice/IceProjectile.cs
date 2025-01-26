using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;
using System;
using System.Linq;

namespace AquaRegia.Weapons.Ice;

public class IceProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    const int BatchSize = 4;
    const int DustSize = 10;
    const float DustScale = 0.8f;

    const float Size = DustSize * DustScale * BatchSize;
    const float HalfSize = Size / 2;

    private List<(List<(Vector2 Position, Timer Timer)> Water, List<Dust> Ice)> _batches;
    private List<(Vector2, float)> _currentBatch;
    private List<KeyValuePair<Rectangle, List<Dust>>> _areas;

    private bool _deactivated = false;

    public IceProjectile() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);

        IsAmmoRuntime = true;

        _batches = new();
        _currentBatch = new();
        _areas = new();
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetDefaults(this, 16, 16, 1, -1);
        Property.SetTimeLeft(this, 70);
        Property.SetGravity();
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Water.ApplyAmmo(_source.Ammo);
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        _deactivated = true;

        return false;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        var count = 0;
        foreach (var (rect, batch) in _areas)
        {
            foreach (var dust in batch)
            {
                count += 1;
                dust.active = false;

                if (count > 2)
                {
                    count = 0;
                    Particle.Single(DustID.Ice, dust.position, new Vector2(2, 2), Main.rand.NextVector2Unit(), 0.8f);
                }
            }
        }
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        if (!_deactivated)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
        }

        var removeQueue = new List<KeyValuePair<Rectangle, List<Dust>>>();
        var didCollide = false;

        foreach (var tuple in _areas)
        {
            if (tuple.Key.Intersects(targetHitbox))
            {
                didCollide = true;

                foreach (var dust in tuple.Value)
                {
                    dust.active = false;
                    Particle.Single(DustID.Ice, dust.position, new Vector2(2, 2), Main.rand.NextVector2Unit(), 0.8f);
                }

                removeQueue.Add(tuple);
            }
        }

        foreach (var tuple in removeQueue)
        {
            _areas.Remove(tuple);
        }

        return didCollide;
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (Main.rand.Next(0, 8) == 0)
        {
            target.AddBuff(BuffID.Frostburn, 120);
            target.AddBuff(BuffID.Slow, 120);
        }

        _deactivated = true;
    }

    public override void AI()
    {
        base.AI();

        var markQueue = new List<(List<(Vector2, Timer)>, List<Dust>)>();

        foreach (var batch in _batches)
        {
            if (batch.Water.Count <= 0)
            {
                var middle = (batch.Ice[(int)MathF.Floor(batch.Ice.Count / 2)].position +
                              batch.Ice[(int)MathF.Ceiling(batch.Ice.Count / 2)].position) /
                             2;

                var batchCopy = new List<Dust>();
                batchCopy.AddRange(batch.Ice);

                _areas.Add(KeyValuePair.Create(
                    new Rectangle((int)(middle.X - HalfSize), (int)(middle.Y - HalfSize), (int)Size, (int)Size),
                    batchCopy));

                markQueue.Add(batch);
            }
            else
            {
                var removeQueue = new List<(Vector2, Timer)>();

                foreach (var dust in batch.Water)
                {
                    dust.Timer.Update();

                    if (dust.Timer.Done)
                    {
                        var particle = Particle.SinglePerfect(ModContent.DustType<IceDust>(), dust.Position,
                                                              Vector2.Zero, DustScale, color: Color.White);

                        batch.Ice.Add(particle);
                        removeQueue.Add(dust);
                    }
                }

                foreach (var dust in removeQueue)
                {
                    batch.Water.Remove(dust);
                }
                removeQueue.Clear();
            }
        }

        foreach (var batch in markQueue)
        {
            _batches.Remove(batch);
        }

        if (!_deactivated && Projectile.timeLeft >= Property.DefaultTime - 35)
        {
            Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
            var dusts = Water.CreateDust(Projectile.Center, Projectile.velocity);

            foreach (var dust in dusts)
            {
                _currentBatch.Add((dust.position, dust.scale));

                if (_currentBatch.Count >= BatchSize)
                {
                    var batchCopy = new List<(Vector2, float)>();
                    batchCopy.AddRange(_currentBatch);
                    _batches.Add(
                        (batchCopy.Select(b => (b.Item1, new Timer((int)(15 * b.Item2)))).ToList(), new List<Dust>()));
                    _currentBatch.Clear();
                }
            }
        }
        else
        {
            Projectile.velocity = Vector2.Zero;
            _deactivated = true;
        }
    }
}
