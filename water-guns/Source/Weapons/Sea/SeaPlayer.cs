using System.Collections.Generic;
using WaterGuns.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace WaterGuns.Weapons.Sea;

public class SeaSource : IEntitySource
{
    public NPC Target;

    public string? Context { get; set; }

    public SeaSource(IEntitySource source, NPC target)
    {
        Context = source.Context;
        Target = target;
    }
}

public class SeaPlayer : ModPlayer
{
    private Dictionary<NPC, HugeBubble> _bubbles = new();

    public int ProjectileDamage { get; set; }

    public bool CanHome(NPC target)
    {
        if (!_bubbles.ContainsKey(target))
            return true;

        return !_bubbles[target].IsMaxStage;
    }

    public void AddBubble(NPC target)
    {
        if (!_bubbles.ContainsKey(target))
        {
            var source = new SeaSource(Projectile.GetSource_NaturalSpawn(), target);
            var projectile = Helper.SpawnProjectile<HugeBubble>(source, Player, target.Center, Vector2.Zero, 0, 0);
            _bubbles[target] = projectile;
        }

        _bubbles[target].Enlarge();
    }

    public bool BubbleCollide(Rectangle rect)
    {
        foreach (var bubble in _bubbles.Values)
        {
            if (bubble.IsMaxStage && bubble.WorldRectangle.Intersects(rect))
            {
                bubble.Explode();

                return true;
            }
        }

        return false;
    }

    public void RemoveBubble(NPC target)
    {
        _bubbles.Remove(target);
    }
}
