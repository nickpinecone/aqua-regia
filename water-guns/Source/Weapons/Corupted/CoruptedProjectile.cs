using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Corupted;

public class CoruptedProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    private int _splitCount = 0;
    private CoruptedSource? _source;

    public CoruptedProjectile() : base()
    {
        var immunity = new ImmunityModule();
        immunity.SetDefaults();
        Composite.AddRuntimeModule(immunity);

        Property = new PropertyModule();
        Water = new WaterModule();

        Composite.AddModule(Property, Water);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults(color: Color.DarkMagenta);
        Property.SetProperties(this, 16, 16, 1, 1);
        Property.SetTimeLeft(this, 16);
        Property.SetGravity(0.01f, 0.015f);
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        if (source is CoruptedSource custom)
        {
            _source = custom;
            custom.Ammo?.ApplyToProjectile(this);
            _splitCount = custom.SplitCount;
        }
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        if (_splitCount < 2 && timeLeft <= 0)
        {
            _source!.SplitCount += 1;

            for (int i = -1; i < 2; i += 2)
            {
                var velocity = Projectile.velocity.RotatedBy(0.16f * i).RotatedByRandom(0.08f);
                var proj = Helper.SpawnProjectile<CoruptedProjectile>(_source!, Owner, Projectile.Center, velocity, Projectile.damage, Projectile.knockBack);
                proj.Projectile.timeLeft -= 4 * _splitCount;
            }
        }

        else
        {
            Water.KillEffect(Projectile.Center, Projectile.velocity);
        }
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Water.CreateDust(Projectile.Center, Projectile.velocity);
    }
}
