using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Weapons.Granite;

public class GraniteProjecitle : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public GraniteProjecitle() : base()
    {
        Property = new PropertyModule();
        Water = new WaterModule();

        Composite.AddModule(Property, Water);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetProperties(this, 16, 16, 1, 1);
        Property.SetGravity();
        Property.SetTimeLeft(this, 35);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        if (source is WeaponWithAmmoSource custom)
        {
            custom.Ammo?.ApplyToProjectile(this);
        }
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Water.KillEffect(Projectile.Center, Projectile.velocity);
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (Main.rand.Next(0, 3) == 0)
        {
            var source = new GraniteSource(Projectile.GetSource_FromThis(), target);

            Helper.SpawnProjectile<GraniteChunk>(source, Owner, target.Center, Vector2.Zero, hit.Damage, 0);
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Water.CreateDust(Projectile.Center, Projectile.velocity);
    }
}
