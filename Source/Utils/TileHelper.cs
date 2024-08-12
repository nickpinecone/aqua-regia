using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Utils;

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

    public static bool IsSolid(Tile tile)
    {
        return tile.HasTile && Main.tileSolid[tile.TileType];
    }

    public static bool IsSolid(Point point)
    {
        return IsSolid(GetTile(point));
    }

    public static bool IsSolid(Vector2 position)
    {
        return IsSolid(position.ToTileCoordinates());
    }

    public static IEnumerable<Point> Area(Vector2 center, int width, int height)
    {
        for (int x = -width; x <= width; x++)
        {
            for (int y = -height; y <= height; y++)
            {
                var position = center + new Vector2(16 * x, 16 * y);

                yield return position.ToTileCoordinates();
            }
        }
    }

    public static bool AnySolidInArea(Vector2 center, int width, int height)
    {
        foreach (var tile in Area(center, width, height))
        {
            if (IsSolid(tile))
            {
                return true;
            }
        }

        return false;
    }

    public static IEnumerable<Point> FromTop(Vector2 from, float amount)
    {
        var tilePosition = from.ToTileCoordinates();
        var tileAmount = amount / 16;

        for (var i = 0; i < tileAmount; i++)
        {
            var position = (from + new Vector2(0, 16 * i));

            yield return position.ToTileCoordinates();
        }
    }

    public static Point? FirstSolidFromTop(Vector2 from, float amount)
    {
        foreach (var tile in FromTop(from, amount))
        {
            if (IsSolid(tile))
            {
                return tile;
            }
        }

        return null;
    }

    public static IEnumerable<Point> ScanSolidSurface(Vector2 center, int width, int height)
    {
        List<Point> surface = new();

        for (int yDirection = -1; yDirection <= 1; yDirection += 2)
        {
            for (int dy = 0; Math.Abs(dy) <= height; dy += yDirection)
            {
                var position = center + new Vector2(0, dy * 16);
                if (IsSolid(position))
                {
                    surface.Add(position.ToTileCoordinates());
                    break;
                }

                for (int xDirection = -1; xDirection <= 1; xDirection += 2)
                {
                    for (int dx = 0; Math.Abs(dx) <= width; dx += xDirection)
                    {
                        position = center + new Vector2(dx * 16, dy * 16);

                        if (IsSolid(position))
                        {
                            surface.Add(position.ToTileCoordinates());
                            break;
                        }
                    }
                }
            }
        }

        for (int xDirection = -1; xDirection <= 1; xDirection += 2)
        {
            for (int dx = 0; Math.Abs(dx) <= width; dx += xDirection)
            {
                var position = center + new Vector2(dx * 16, 0);
                if (IsSolid(position))
                {
                    surface.Add(position.ToTileCoordinates());
                    break;
                }

                for (int yDirection = -1; yDirection <= 1; yDirection += 2)
                {
                    for (int dy = 0; Math.Abs(dy) <= height; dy += yDirection)
                    {
                        position = center + new Vector2(dx * 16, dy * 16);

                        if (IsSolid(position))
                        {
                            surface.Add(position.ToTileCoordinates());
                            break;
                        }
                    }
                }
            }
        }

        return surface;
    }
}
