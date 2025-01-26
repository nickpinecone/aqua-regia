using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.Golden;

public class GoldenPlayer : ModPlayer
{
    private List<SwordProjectile> _swords = new();

    public void SpawnSword(Player player, int damage, float knockback)
    {
        var aim = Main.MouseWorld - player.Center;
        aim.Normalize();
        var direction = aim.X > 0 ? 1 : -1;

        var offsetY = Main.rand.NextFloat(-96f, 96f);
        var offsetX = Main.rand.NextFloat(0, 96f) * -direction;
        var offsetVector = new Vector2(offsetX, offsetY);
        var position = player.Center + offsetVector;

        var sword =
            Projectile
                .NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), position, Vector2.Zero,
                                     ModContent.ProjectileType<SwordProjectile>(), damage, knockback, Main.myPlayer)
                .ModProjectile as SwordProjectile;

        var relative = Main.MouseWorld - position;
        var velocity = (new Vector2(1, 0) * 16f).RotatedBy(relative.ToRotation());
        sword.InitialVelocity = velocity;
        sword.Projectile.rotation = velocity.ToRotation() + MathHelper.PiOver4;

        _swords.Add(sword);
    }

    public void RemoveSword(SwordProjectile sword)
    {
        _swords.Remove(sword);
    }

    public bool SwordCollide(Rectangle rect)
    {
        foreach (var sword in _swords)
        {
            if (sword.WorldRectangle.Intersects(rect))
            {
                sword.Push();

                return true;
            }
        }

        return false;
    }
}
