using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AquaRegia.Library.Extended.Helpers;

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

    public static bool IsWater(Tile tile, int amount = 255)
    {
        return tile.LiquidType == LiquidID.Water && tile.LiquidAmount == amount;
    }

    public static bool IsWater(Point point, int amount = 255)
    {
        return IsWater(GetTile(point), amount);
    }

    public static bool IsWater(Vector2 position, int amount = 255)
    {
        return IsWater(GetTile(position), amount);
    }

    public static bool IsSolid(Tile tile, bool alsoSolidTop = false)
    {
        if (alsoSolidTop)
        {
            return tile.HasTile && Main.tileSolid[tile.TileType];
        }

        return tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
    }

    public static bool IsSolid(Point point, bool alsoSolidTop = false)
    {
        return IsSolid(GetTile(point), alsoSolidTop);
    }

    public static bool IsSolid(Vector2 position, bool alsoSolidTop = false)
    {
        return IsSolid(GetTile(position), alsoSolidTop);
    }
}