using System;
using System.Collections.Generic;
using AquaRegia.Library.Extended.Global;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Biomes.Forest;

public class ForestBiome : ModSystem
{
    public override void Load()
    {
        base.Load();

        NpcGlobal.EditSpawnPoolEvent += OnEditSpawnPoolEvent;
    }

    private static void OnEditSpawnPoolEvent(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        if (Main.LocalPlayer.ZoneForest && spawnInfo.Water)
        {
            pool.Add(NPCID.BlueJellyfish, 1f);
            pool.Add(NPCID.PinkJellyfish, 1f);
            pool.Add(NPCID.GreenJellyfish, 1f);
        }
    }
}