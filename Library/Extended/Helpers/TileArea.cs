using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Helpers;

public static class TileArea
{
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
        return Area(center, width, height).Any(tile => TileHelper.IsSolid(tile, alsoSolidTop));
    }

    public static IEnumerable<Point> ScanSolidSurface(Vector2 center, int width, int height, bool alsoSolidTop = false)
    {
        List<Point> surface = [];

        for (var yDirection = -1; yDirection <= 1; yDirection += 2)
        {
            for (var dy = 0; Math.Abs(dy) <= height; dy += yDirection)
            {
                var position = center + new Vector2(0, dy * 16);

                if (TileHelper.IsSolid(position, alsoSolidTop))
                {
                    surface.Add(position.ToTileCoordinates());
                    break;
                }

                for (var xDirection = -1; xDirection <= 1; xDirection += 2)
                {
                    for (var dx = 0; Math.Abs(dx) <= width; dx += xDirection)
                    {
                        position = center + new Vector2(dx * 16, dy * 16);

                        if (!TileHelper.IsSolid(position, alsoSolidTop)) continue;

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
                if (TileHelper.IsSolid(position, alsoSolidTop))
                {
                    surface.Add(position.ToTileCoordinates());
                    break;
                }

                for (var yDirection = -1; yDirection <= 1; yDirection += 2)
                {
                    for (var dy = 0; Math.Abs(dy) <= height; dy += yDirection)
                    {
                        position = center + new Vector2(dx * 16, dy * 16);

                        if (!TileHelper.IsSolid(position, alsoSolidTop)) continue;

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
            if (TileHelper.IsSolid(start))
            {
                return true;
            }

            start = start.MoveTowards(end, 16f);
        }

        return false;
    }
}