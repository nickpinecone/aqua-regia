using System.Collections.Generic;
using System.Linq;
using AquaRegia.Utils;
using AquaRegia.Weapons.Sea;
using AquaRegia.World.CoralReef.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

public class CoralPlayer : ModPlayer
{
    private Dictionary<Point, bool> _breathSpots = new();
    private Timer _timer = new(120);

    public void AddBreathSpot(Point position)
    {
        if (!_breathSpots.ContainsKey(position))
        {
            var topTile = Main.tile[position.X, position.Y - 1];

            if (topTile.HasTile)
            {
                _breathSpots.Add(new Point(position.X, position.Y), false);
            }
            else
            {
                _breathSpots.Add(position, Main.rand.Next(0, 15) == 0);
            }
        }
    }

    public override void PreUpdate()
    {
        _timer.Update();

        if (_timer.Done)
        {
            foreach (var spot in _breathSpots.Where(b => b.Value == true))
            {

                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), spot.Key.ToWorldCoordinates(),
                                         -Vector2.UnitY, ModContent.ProjectileType<BubbleProjectile>(), 0, 0f);
            }

            _timer.Restart();
        }
    }
}
