using System.Collections.Generic;
using AquaRegia.Library.Helpers;
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
        WorldGenHelper.RemoveGenPass(tasks, "Jungle Trees");
        WorldGenHelper.RemoveGenPass(tasks, "Sunflowers");
        WorldGenHelper.RemoveGenPass(tasks, "Herbs");
        WorldGenHelper.RemoveGenPass(tasks, "Dye Plants");
        WorldGenHelper.RemoveGenPass(tasks, "Weeds");
        WorldGenHelper.RemoveGenPass(tasks, "Glowing Mushrooms and Jungle Plants");
        WorldGenHelper.RemoveGenPass(tasks, "Jungle Plants");
        WorldGenHelper.RemoveGenPass(tasks, "Flowers");
        WorldGenHelper.RemoveGenPass(tasks, "Mushrooms");
        WorldGenHelper.RemoveGenPass(tasks, "Cactus, Palm Trees, & Coral");

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

        for (var i = 0; i < treeCount; i++)
        {
            var point = start + new Point(WorldGen.genRand.Next(-24, 24), 0);
            var depth = 0;

            while (!TileHelper.IsSolid(point) && depth < 16)
            {
                depth += 1;
                point += new Point(0, 1);
            }

            point -= new Point(0, 1);
            WorldGenHelper.PlaceTree(point.X, point.Y);
        }
    }
}