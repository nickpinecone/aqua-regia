using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WaterGuns.Projectiles.Golden;
using WaterGuns.Utils;

namespace WaterGuns.Players;

public class GoldenPlayer : ModPlayer
{
    private List<SwordProjectile> _swords = new();

    public int Damage { get; set; }
    public int Knockback { get; set; }

    public Timer SpawnTimer { get; private set; }

    public GoldenPlayer()
    {
        SpawnTimer = new Timer(40, this, false);
    }

    private void NewSword()
    {
        var offset = 320f;
        var offsetVector = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(offset, offset + 12f);
        var position = Main.MouseWorld + offsetVector;

        var sword =
            Projectile
                .NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), position, Vector2.Zero,
                                     ModContent.ProjectileType<SwordProjectile>(), Damage, Knockback, Main.myPlayer)
                .ModProjectile as SwordProjectile;

        offsetVector.Normalize();
        offsetVector = offsetVector.RotatedBy(MathHelper.Pi);
        sword.InitialVelocity = offsetVector * 16f;
        sword.Projectile.rotation = offsetVector.ToRotation() + MathHelper.PiOver4;

        _swords.Add(sword);
    }

    public void SpawnSwords()
    {
        NewSword();
        SpawnTimer.Restart();
    }

    public void RemoveSword(SwordProjectile sword)
    {
        _swords.Remove(sword);
    }

    public bool SwordCollide(Rectangle rect)
    {
        foreach (var sword in _swords)
        {
            if (sword.Projectile.getRect().Intersects(rect))
            {
                sword.Push();

                return true;
            }
        }

        return false;
    }

    public override void PreUpdate()
    {
        base.PreUpdate();

        SpawnTimer.Update();

        if (SpawnTimer.Done)
        {
            NewSword();

            SpawnTimer.Restart();
            SpawnTimer.Paused = true;
        }
    }
}
