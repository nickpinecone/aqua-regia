using System.Collections.Generic;
using AquaRegia.Library.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
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

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
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

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = IslandSystem.SpawnGenMessage?.Value;
        
        var position = new Point(Main.spawnTileX, (int)UnderwaterSystem.TileSeaLevel);
        while (TileHelper.IsSolid(position))
        {
            position.Y -= 1;
        }

        Main.spawnTileX = (int)position.X;
        Main.spawnTileY = (int)position.Y;
    }
}