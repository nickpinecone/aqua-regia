using AquaRegia.Library.Extended.Global;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Tiles.Seaweed;

public class Seaweed : ModSystem
{
    public override void Load()
    {
        base.Load();

        TileGlobal.CanDropEvent += OnCanDropEvent;
        TileGlobal.KillTileEvent += OnKillTileEvent;
        TileGlobal.DropEvent += OnDropEvent;
    }

    private static void OnDropEvent(int i, int j, int type)
    {
        if (type == TileID.Seaweed)
        {
            Item.NewItem(Entity.GetSource_NaturalSpawn(), i * 16, j * 16, 0, 0, ItemID.FishingSeaweed);
        }
    }

    private static void OnKillTileEvent(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (type == TileID.Seaweed)
        {
            noItem = false;
        }
    }

    private static bool OnCanDropEvent(int i, int j, int type)
    {
        return type == TileID.Seaweed;
    }
}