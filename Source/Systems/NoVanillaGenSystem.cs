using System.Collections.Generic;
using AquaRegia.Library.Helpers;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.Systems;

public class NoVanillaGenSystem : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        // Plants
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
        
        // Biomes and structures
        WorldGenHelper.RemoveGenPass(tasks, "Underworld");
        WorldGenHelper.RemoveGenPass(tasks, "Hellforge");
        WorldGenHelper.RemoveGenPass(tasks, "Dungeon");
        WorldGenHelper.RemoveGenPass(tasks, "Pyramids");
        WorldGenHelper.RemoveGenPass(tasks, "Jungle Temple");
        WorldGenHelper.RemoveGenPass(tasks, "Hives");
        WorldGenHelper.RemoveGenPass(tasks, "Temple");
        WorldGenHelper.RemoveGenPass(tasks, "Larva");
        WorldGenHelper.RemoveGenPass(tasks, "Lihzahrd Altars");
        WorldGenHelper.RemoveGenPass(tasks, "Generate Ice Biome");
        WorldGenHelper.RemoveGenPass(tasks, "Jungle");
        WorldGenHelper.RemoveGenPass(tasks, "Full Desert");
        WorldGenHelper.RemoveGenPass(tasks, "Corruption");
        WorldGenHelper.RemoveGenPass(tasks, "Wet Jungle");
        WorldGenHelper.RemoveGenPass(tasks, "Ice");
        WorldGenHelper.RemoveGenPass(tasks, "Gems In Ice Biome");
        WorldGenHelper.RemoveGenPass(tasks, "Jungle Chests");
        WorldGenHelper.RemoveGenPass(tasks, "Buried Chests");
        WorldGenHelper.RemoveGenPass(tasks, "Surface Chests");
        WorldGenHelper.RemoveGenPass(tasks, "Jungle Chests Placement");
        WorldGenHelper.RemoveGenPass(tasks, "Water Chests");
    }
}