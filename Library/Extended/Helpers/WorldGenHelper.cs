using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace AquaRegia.Library.Helpers;

public static class WorldGenHelper
{
    public static bool RemoveGenPass(List<GenPass> tasks, string name)
    {
        var index = tasks.FindIndex(genpass => genpass.Name.Equals(name));

        if (index != -1)
        {
            tasks.RemoveAt(index);
        }

        return index != -1;
    }

    public static bool ReplaceGenPass(List<GenPass> tasks, string name, GenPass pass)
    {
        var index = tasks.FindIndex(genpass => genpass.Name.Equals(name));

        if (index != -1)
        {
            tasks.Insert(index, pass);
            tasks.RemoveAt(index + 1);
        }

        return index != -1;
    }

    public static bool InsertAfterGenPass(List<GenPass> tasks, string name, GenPass pass)
    {
        var index = tasks.FindIndex(genpass => genpass.Name.Equals(name));

        if (index != -1)
        {
            tasks.Insert(index + 1, pass);
        }

        return index != -1;
    }

    public static bool InsertBeforeGenPass(List<GenPass> tasks, string name, GenPass pass)
    {
        var index = tasks.FindIndex(genpass => genpass.Name.Equals(name));

        if (index != -1)
        {
            tasks.Insert(index, pass);
        }

        return index != -1;
    }

    public static bool PlaceTree(int x, int y)
    {
        WorldGen.PlaceTile(x, y, TileID.Saplings);
        var success = WorldGen.GrowTree(x, y);
        if (!success) WorldGen.KillTile(x, y);
        return success;
    }
}