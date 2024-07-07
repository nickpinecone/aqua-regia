using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Utils;

public static class TileHelper
{
    public static bool IsSolid(Tile tile)
    {
        return tile.HasTile && Main.tileSolid[tile.TileType];
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

    public static IEnumerable<Point> FromTop(Vector2 from, float to)
    {
        var tilePosition = from.ToTileCoordinates();
        var tileAmount = (to - from.Y) / 16;

        for(var i = 0; i < tileAmount; i++)
        {
            tilePosition = (from + new Vector2(0, 16 * i)).ToTileCoordinates();

            yield return tilePosition;
        }
    }
}
