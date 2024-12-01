using System.Collections.Generic;
using System.Linq;
using AquaRegia.Utils;
using AquaRegia.World.CoralReef.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

public class CoralPlayer : ModPlayer
{
    private Dictionary<Point, (bool IsBreath, Timer Timer)> _breathSpots = new();

    public void AddBreathSpot(Point position)
    {
        if (!_breathSpots.ContainsKey(position))
        {
            var topTile = Main.tile[position.X, position.Y - 1];

            if (topTile.HasTile)
            {
                _breathSpots.Add(new Point(position.X, position.Y), (false, null));
            }
            else
            {
                var isBreath = Main.rand.Next(0, 200) == 0;
                var timer = isBreath ? new Timer(Main.rand.Next(30, 60)) : null;
                _breathSpots.Add(position, (isBreath, timer));
            }
        }
    }

    public override void PreUpdate()
    {
        foreach (var spot in _breathSpots.Where(b => b.Value.IsBreath))
        {
            spot.Value.Timer.Update();

            if (spot.Value.Timer.Done)
            {
                spot.Value.Timer.WaitTime = Main.rand.Next(30, 60);
                spot.Value.Timer.Restart();

                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), spot.Key.ToWorldCoordinates(),
                                         Vector2.Zero, ModContent.ProjectileType<BreathBubble>(), 0, 0f);
            }
        }
    }
}
