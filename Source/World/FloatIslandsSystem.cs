using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.World;

public class FloatIslandsSystem : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        var floatIslandsIndex = tasks.FindIndex(genPass => genPass.Name.Equals("Floating Islands"));

        if (floatIslandsIndex != -1)
        {
            tasks.RemoveAt(floatIslandsIndex);
        }
    }
}