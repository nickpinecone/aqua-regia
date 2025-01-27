using System.Collections.Generic;
using AquaRegia.Utils;
using AquaRegia.World.CoralReef.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.World.CoralReef;

public class CoralCleanupPass : GenPass
{
    public CoralCleanupPass(string name, float loadWeight) : base(name, loadWeight)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        var coralTiles = new List<Point>();

        for (int x = 1; x < Main.maxTilesX; x++)
        {
            for (int y = 1; y < Main.maxTilesY; y++)
            {
                var tile = Main.tile[x, y];

                if (tile.HasTile && tile.TileType == ModContent.TileType<CoralTile>())
                {
                    coralTiles.Add(new Point(x, y));

                    var topPos = new Point(x, y - 1);
                    var topTile = Main.tile[topPos.X, topPos.Y];

                    if (!TileHelper.IsSolid(topPos))
                    {
                        WorldGen.KillTile(topPos.X, topPos.Y, noItem: true);

                        if (WorldGen.genRand.Next(0, 15) == 0)
                        {
                            var type = WorldGen.genRand.NextFromList(TileID.Coral, TileID.BeachPiles);
                            WorldGen.PlaceTile(topPos.X, topPos.Y, type);
                        }
                    }
                }
            }
        }
    }
}
