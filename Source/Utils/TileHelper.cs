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
}
