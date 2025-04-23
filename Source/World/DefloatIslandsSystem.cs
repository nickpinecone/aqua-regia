using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.World;

public class DefloatIslandsSystem : ModSystem
{
    public static LocalizedText? DefloatIslandsGenMessage { get; private set; }

    public override void SetStaticDefaults()
    {
        DefloatIslandsGenMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(DefloatIslandsGenMessage)}"));
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int FloatIslandsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Islands"));

        if (FloatIslandsIndex != -1)
        {
            tasks.Insert(FloatIslandsIndex, new DefloatIslandsGenPass("Defloating Islands", 100f));
        }

        tasks.RemoveAt(FloatIslandsIndex + 1);
    }
}

public class DefloatIslandsGenPass : GenPass
{
    public DefloatIslandsGenPass(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    private static int GetIslandCount()
    {
        return WorldGen.GetWorldSize() switch
        {
            0 => WorldGen.genRand.Next(2, 4),
            1 => WorldGen.genRand.Next(3, 6),
            2 => WorldGen.genRand.Next(5, 9),
            _ => WorldGen.genRand.Next(6, 12),
        };
    }

    public static void Defloat()
    {
        var islandCount = GetIslandCount();

        var spaceBetween = Main.maxTilesX / islandCount;
        var diff = (int)(Main.maxTilesX * 0.05f);
        var x = 0;

        for (int i = 0; i < islandCount; i++)
        {
            x += spaceBetween + WorldGen.genRand.Next(-diff, diff);
            var y = FloodSystem.FloodLevel;

            WorldGen.FloatingIsland(x, y);
        }
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        Defloat();
    }
}