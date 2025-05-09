using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.World;

public class UnderworldSystem : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        var underworldIndex = tasks.FindIndex(genPass => genPass.Name.Equals("Underworld"));

        if (underworldIndex != -1)
        {
            tasks.RemoveAt(underworldIndex);
        }
        
        var hellforgeIndex = tasks.FindIndex(genPass => genPass.Name.Equals("Hellforge"));

        if (hellforgeIndex != -1)
        {
            tasks.RemoveAt(hellforgeIndex);
        }
    }
}