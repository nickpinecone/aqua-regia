using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;
using System;

namespace AquaRegia.Weapons.Ice;

public class IceProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    private List<Dust> _waterDusts;
    private List<Dust> _removeQueue;

    private List<KeyValuePair<Rectangle, List<Dust>>> _areas;
    private bool _deactivated = false;

    public IceProjectile() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);

        IsAmmoRuntime = true;

        _waterDusts = new();
        _removeQueue = new();
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

    public void Deactivate()
    {
        Projectile.velocity = Vector2.Zero;
        _deactivated = true;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Water.ApplyAmmo(_source.Ammo);
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Deactivate();

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
        var index = 0;

        foreach (var (rect, batch) in _areas)
        {
            if (rect.Intersects(targetHitbox))
            {
                foreach (var dust in batch)
                {
                    dust.active = false;
                    Particle.Single(DustID.Ice, dust.position, new Vector2(2, 2), Main.rand.NextVector2Unit(), 0.8f);
                }
                _areas.RemoveAt(index);

                return true;
            }
            index++;
        }

        return false;
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        Deactivate();
    }

    const int BatchSize = 5;
    const int DustSize = 10;
    const float Scale = 0.8f;

    const float Size = DustSize * Scale * BatchSize;
    const float HalfSize = Size / 2;

    List<Dust> batch = new List<Dust>();

    public override void AI()
    {
        base.AI();

        foreach (var dust in _waterDusts)
        {
            // if (dust.active == false)
            // {
            var particle = Particle.SinglePerfect(ModContent.DustType<IceDust>(), dust.position, Vector2.Zero, 0.8f,
                                                  color: Color.White);

            batch.Add(particle);

            if (batch.Count >= BatchSize)
            {
                var middle = (batch[(int)MathF.Floor(batch.Count / 2)].position +
                              batch[(int)MathF.Ceiling(batch.Count / 2)].position) /
                             2;

                var batchCopy = new List<Dust>();
                batchCopy.AddRange(batch);

                _areas.Add(KeyValuePair.Create(
                    new Rectangle((int)(middle.X - HalfSize), (int)(middle.Y - HalfSize), (int)Size, (int)Size),
                    batchCopy));

                batch.Clear();
            }

            _removeQueue.Add(dust);
            // }
        }

        foreach (var dust in _removeQueue)
        {
            _waterDusts.Remove(dust);
        }
        _removeQueue.Clear();

        if (_deactivated || Projectile.timeLeft <= Property.DefaultTime - 35)
        {
            Deactivate();
        }
        else
        {
            Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
            var dusts = Water.CreateDust(Projectile.Center, Projectile.velocity);
            _waterDusts.AddRange(dusts);
        }
    }
}
