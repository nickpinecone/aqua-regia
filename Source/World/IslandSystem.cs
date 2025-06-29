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

public class IslandSystem : ModSystem
{
    public static LocalizedText? IslandGenMessage { get; private set; }
    public static LocalizedText? SpawnGenMessage { get; private set; }

    public override void SetStaticDefaults()
    {
        IslandGenMessage =
            Language.GetOrRegister(Mod.GetLocalizationKey($"World.{nameof(IslandGenMessage)}"));

        SpawnGenMessage =
            Language.GetOrRegister(Mod.GetLocalizationKey($"World.{nameof(SpawnGenMessage)}"));
    }

    // TODO this could also be in WorldGenHelper
    private void RemovePass(List<GenPass> tasks, string name)
    {
        var index = tasks.FindIndex(genpass => genpass.Name.Equals(name));

        if (index != -1)
        {
            tasks.RemoveAt(index);
        }
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        RemovePass(tasks, "Jungle Trees");
        RemovePass(tasks, "Planting Trees");
        RemovePass(tasks, "Cactus, Palm Trees, & Coral");

        var floatingIslands = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Islands"));

        if (floatingIslands != -1)
        {
            tasks.Insert(floatingIslands, new IslandGenPass("Spawn Island", 100f));
            tasks.RemoveAt(floatingIslands + 1);
        }

        var spawnPoint = tasks.FindIndex(genpass => genpass.Name.Equals("Spawn Point"));

        if (spawnPoint != -1)
        {
            tasks.Insert(spawnPoint + 1, new SpawnGenPass("Island Spawn Point", 100f));
        }
    }
}

public class IslandGenPass : GenPass
{
    public IslandGenPass(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = IslandSystem.IslandGenMessage?.Value;

        var position = new Vector2((int)(Main.maxTilesX / 2), (int)UnderwaterSystem.TileSeaLevel);
        WorldGen.FloatingIsland((int)position.X, (int)position.Y);
    }
}

public class SpawnGenPass : GenPass
{
    public SpawnGenPass(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    // TODO Probably make a WorldGenHelper
    private static void PlaceTree(int x, int y)
    {
        WorldGen.PlaceTile(x, y, TileID.Saplings);
        WorldGen.GrowTree(x, y);
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = IslandSystem.SpawnGenMessage?.Value;

        var position = new Point(Main.spawnTileX, (int)UnderwaterSystem.TileSeaLevel);
        while (TileHelper.IsSolid(position))
        {
            position.Y -= 1;
        }

        // TODO place them randomly on the island
        // also should be moved to an earlier stage, probably replace with Placing Trees step
        PlaceTree(position.X, position.Y);

        Main.spawnTileX = position.X;
        Main.spawnTileY = position.Y;
    }
}