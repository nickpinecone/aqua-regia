using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WaterGuns.Projectiles.Sea;
using WaterGuns.Utils;

namespace WaterGuns.Players;

public class SeaPlayer : ModPlayer
{
    public Dictionary<NPC, HugeBubble> _bubbles { get; private set; }

    public SeaPlayer()
    {
        _bubbles = new();
    }

    public bool CanHome(NPC target)
    {
        if (!_bubbles.ContainsKey(target))
            return true;

        return !_bubbles[target].IsMaxSize;
    }

    public void Enlarge(NPC target)
    {
        if (!_bubbles.ContainsKey(target))
        {
            var projectile =
                Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), target.Center, Vector2.Zero,
                                               ModContent.ProjectileType<HugeBubble>(), 1, 0, Main.myPlayer);

            _bubbles[target] = projectile.ModProjectile as HugeBubble;
            _bubbles[target].Target = target;
        }
        _bubbles[target].Enlarge();
    }

    public bool CheckCollision(Rectangle rect)
    {
        foreach(var bubble in _bubbles.Values)
        {
            if(bubble.IsMaxSize && bubble.WorldRectangle.Intersects(rect))
            {
                bubble.Explode();

                return true;
            }
        }

        return false;
    }

    public void Remove(NPC target)
    {
        _bubbles.Remove(target);
    }
}
