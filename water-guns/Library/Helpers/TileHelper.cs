using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Library.Helpers;

public static class TileHelper
{
    public static Tile GetTile(Point point)
    {
        return Main.tile[point.X, point.Y];
    }

    public static Tile GetTile(Vector2 position)
    {
        return GetTile(position.ToTileCoordinates());
    }

    public static bool IsSolid(Tile tile, bool alsoSolidTop = false)
    {
        if (alsoSolidTop)
        {
            return tile.HasTile && Main.tileSolid[tile.TileType];
        }
        else
        {
            return tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
        }
    }

    public static bool IsSolid(Point point, bool alsoSolidTop = false)
    {
        return IsSolid(GetTile(point), alsoSolidTop);
    }

    public static bool IsSolid(Vector2 position, bool alsoSolidTop = false)
    {
        return IsSolid(position.ToTileCoordinates(), alsoSolidTop);
    }

    public static IEnumerable<Point> Area(Vector2 center, int width, int height)
    {
        for (var x = -width; x <= width; x++)
        {
            for (var y = -height; y <= height; y++)
            {
                var position = center + new Vector2(16 * x, 16 * y);

                yield return position.ToTileCoordinates();
            }
        }
    }

    public static bool AnySolidInArea(Vector2 center, int width, int height, bool alsoSolidTop = false)
    {
        return Area(center, width, height).Any(tile => IsSolid(tile, alsoSolidTop));
    }

    public static IEnumerable<Point> FromTop(Vector2 from, float amount)
    {
        var tileAmount = amount / 16;

        for (var i = 0; i < tileAmount; i++)
        {
            var position = (from + new Vector2(0, 16 * i));

            yield return position.ToTileCoordinates();
        }
    }

    public static Point? FirstSolidFromTop(Vector2 from, float amount, bool alsoSolidTop = false)
    {
        foreach (var tile in FromTop(from, amount))
        {
            if (IsSolid(tile, alsoSolidTop))
            {
                return tile;
            }
        }

        return null;
    }

    public static IEnumerable<Point> ScanSolidSurface(Vector2 center, int width, int height, bool alsoSolidTop = false)
    {
        List<Point> surface = [];

        for (var yDirection = -1; yDirection <= 1; yDirection += 2)
        {
            for (var dy = 0; Math.Abs(dy) <= height; dy += yDirection)
            {
                var position = center + new Vector2(0, dy * 16);
                
                if (IsSolid(position, alsoSolidTop))
                {
                    surface.Add(position.ToTileCoordinates());
                    break;
                }

                for (var xDirection = -1; xDirection <= 1; xDirection += 2)
                {
                    for (var dx = 0; Math.Abs(dx) <= width; dx += xDirection)
                    {
                        position = center + new Vector2(dx * 16, dy * 16);

                        if (!IsSolid(position, alsoSolidTop)) continue;
                        
                        surface.Add(position.ToTileCoordinates());
                        break;
                    }
                }
            }
        }

        for (var xDirection = -1; xDirection <= 1; xDirection += 2)
        {
            for (var dx = 0; Math.Abs(dx) <= width; dx += xDirection)
            {
                var position = center + new Vector2(dx * 16, 0);
                if (IsSolid(position, alsoSolidTop))
                {
                    surface.Add(position.ToTileCoordinates());
                    break;
                }

                for (var yDirection = -1; yDirection <= 1; yDirection += 2)
                {
                    for (var dy = 0; Math.Abs(dy) <= height; dy += yDirection)
                    {
                        position = center + new Vector2(dx * 16, dy * 16);

                        if (!IsSolid(position, alsoSolidTop)) continue;
                        
                        surface.Add(position.ToTileCoordinates());
                        break;
                    }
                }
            }
        }

        return surface;
    }

    public static bool AnySolidInSight(Vector2 start, Vector2 end)
    {
        while (start.DistanceSQ(end) > 16f * 16f)
        {
            if (IsSolid(start))
            {
                return true;
            }

            start = start.MoveTowards(end, 16f);
        }

        return false;
    }
}
