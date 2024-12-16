using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;

namespace AquaRegia.Weapons.Ice;

public class IceProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    private List<Dust> _dusts;
    private List<Dust> _removeQueue;
    private List<Dust> _ice;

    public IceProjectile() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);

        IsAmmoRuntime = true;

        _dusts = new();
        _removeQueue = new();
        _ice = new();
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetDefaults(this, 16, 16, 1, 1);
        Property.SetTimeLeft(this, 70);
        Property.SetGravity();
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Water.ApplyAmmo(_source.Ammo);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        var count = 0;
        foreach (var dust in _ice)
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

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);
    }

    public override void AI()
    {
        base.AI();

        foreach (var dust in _dusts)
        {
            if (dust.active == false)
            {
                var particle = Particle.SinglePerfect(ModContent.DustType<IceDust>(), dust.position, Vector2.Zero, 0.8f,
                                                      color: Color.White);
                _ice.Add(particle);
                _removeQueue.Add(dust);
            }
        }

        foreach (var dust in _removeQueue)
        {
            _dusts.Remove(dust);
        }
        _removeQueue.Clear();

        if (Projectile.timeLeft <= 35)
        {
            Projectile.velocity = Vector2.Zero;
        }
        else
        {
            Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
            var dusts = Water.CreateDust(Projectile.Center, Projectile.velocity);
            _dusts.AddRange(dusts);
        }
    }
}
