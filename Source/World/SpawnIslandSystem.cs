using System;
using System.Collections.Generic;
using AquaRegia.Library.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.World;

public class SpawnIslandSystem : ModSystem
{
    public static LocalizedText? SpawnIslandGenMessage { get; private set; }

    public override void SetStaticDefaults()
    {
        SpawnIslandGenMessage =
            Language.GetOrRegister(Mod.GetLocalizationKey($"World.{nameof(SpawnIslandGenMessage)}"));
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        var islandsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Islands"));

        if (islandsIndex != -1)
        {
            tasks.Insert(islandsIndex, new SpawnIslandGenPass("Spawn Island", 100f));
            tasks.RemoveAt(islandsIndex + 1);
        }

        var spawnIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Spawn Point"));

        if (spawnIndex != -1)
        {
            tasks.Insert(spawnIndex + 1, new SpawnIslandGenPass("Island Spawn Point", 100f));
        }
    }

    // Extra fancy looking water simulation 
    public override void PostDrawTiles()
    {
        base.PostDrawTiles();

        var isInWater = Main.LocalPlayer.Center.Y > WorldConstants.FloodLevel * 16;

        Main.spriteBatch.Begin();
        var diffY = WorldConstants.FloodLevel * 16 - Main.screenPosition.Y;
        diffY = Math.Max(0, diffY);
        
        Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, (int)diffY, 2000, 2000),
            new Color(0, 0, 255, isInWater ? 0.4f : 1f));
        
        Main.spriteBatch.End();
    }
}

public class SpawnIslandGenPass : GenPass
{
    public SpawnIslandGenPass(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        var position = new Vector2(Main.spawnTileX, (int)WorldConstants.FloodLevel);
        WorldGen.FloatingIsland((int)position.X, (int)position.Y);
    }
}

public class SpawnPointGenPass : GenPass
{
    public SpawnPointGenPass(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        var position = new Vector2(Main.spawnTileX, (int)WorldConstants.FloodLevel);

        while (TileHelper.IsSolid(position))
        {
            position.Y -= 16;
        }

        Main.spawnTileX = (int)position.X;
        Main.spawnTileY = (int)position.Y;
    }
}