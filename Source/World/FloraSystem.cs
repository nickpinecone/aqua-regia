using System.Collections.Generic;
using AquaRegia.Library.Extended.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.World;

public class FloraSystem : ModSystem
{
    public static LocalizedText? PlantIslandTreesGenMessage { get; private set; }

    public override void SetStaticDefaults()
    {
        PlantIslandTreesGenMessage = LocalizationHelper.GetLocalization($"World.{nameof(PlantIslandTreesGenMessage)}");
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        WorldGenHelper.ReplaceGenPass(tasks, "Planting Trees",
            new PlantIslandTressGenPass("Planting Island Trees", 100f));
    }
}

public class PlantIslandTressGenPass : GenPass
{
    public PlantIslandTressGenPass(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = FloraSystem.PlantIslandTreesGenMessage?.Value;

        var islandCenter = new Point(Main.spawnTileX, Main.spawnTileY);
        var treeCount = WorldGen.genRand.Next(3, 6);
        var start = islandCenter - new Point(0, 8);

        var i = 0;
        while (i < treeCount)
        {
            var point = start + new Point(WorldGen.genRand.Next(-24, 24), 0);
            var depth = 0;

            while (!TileHelper.IsSolid(point) && depth < 16)
            {
                depth += 1;
                point += new Point(0, 1);
            }

            point -= new Point(0, 1);
            var success = WorldGenHelper.PlaceTree(point.X, point.Y);
            i += success ? 1 : 0;
        }
    }
}