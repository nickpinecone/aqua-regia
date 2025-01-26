using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Terraria.ID;

namespace AquaRegia.Weapons.Sunflower;

public class SunflowerProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public SunflowerProjectile() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);

        IsAmmoRuntime = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetDefaults(this, 16, 16, 1, 1);
        Property.SetTimeLeft(this, 25);
        Property.SetGravity();
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        if (!Main.IsItDay())
        {
            IsAmmoRuntime = false;
            Water.ParticleID = DustID.Blood;

            Projectile.damage += 2;
            Projectile.timeLeft -= 5;
        }
        else
        {
            Water.ApplyAmmo(_source.Ammo);
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
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Water.CreateDust(Projectile.Center, Projectile.velocity);
    }
}
