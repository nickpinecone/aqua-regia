using System;
using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Sea;

public class SeaProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    private SeaPlayer _seaPlayer = null;

    public SeaProjectile() : base()
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
        Property.SetGravity();
        Property.SetTimeLeft(this, 35);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);
        Water.ApplyAmmo(_source.Ammo);

        _seaPlayer = Main.LocalPlayer.GetModPlayer<SeaPlayer>();
        _seaPlayer.ProjectileDamage = Projectile.damage;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Water.KillEffect(Projectile.Center, Projectile.velocity);
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        Vector2 position = Projectile.Center;

        // Finite tries so it does not stuck in a while loop forever
        for (int i = 0; i < 16; i++)
        {
            var offset = MathF.Max(target.width, target.height) * 1.5f;
            position = target.Center +
                       Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(offset, offset + 12f);

            if (!TileHelper.AnySolidInArea(position, 1, 1))
            {
                break;
            }
        }

        SpawnProjectile<BubbleProjectile>(position, Vector2.Zero, 1, 0);
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Water.CreateDust(Projectile.Center, Projectile.velocity);

        if (_seaPlayer.BubbleCollide(Projectile.getRect()))
        {
            Projectile.Kill();
        }
    }
}
