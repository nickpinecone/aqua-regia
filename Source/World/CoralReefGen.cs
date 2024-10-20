using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AquaRegia.Utils;
using System.Collections.Generic;
using System;
using Terraria.WorldBuilding;
using Terraria.Localization;
using Terraria.IO;

namespace AquaRegia.World;

public class CoralReefGen : ModSystem
{
    public static LocalizedText CoralReefGenMessage { get; private set; }
    public int CoralTileCount { get; set; }

    public override void SetStaticDefaults()
    {
        CoralReefGenMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(CoralReefGenMessage)}"));
    }

    public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
    {
        CoralTileCount =
            tileCounts[TileID.RedStucco] + tileCounts[TileID.YellowStucco] + tileCounts[TileID.GreenStucco];
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        if (!ModContent.GetInstance<AquaRegiaConfig>().CoralReefGenEnabled)
        {
            return;
        }

        int OceanCavesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Create Ocean Caves"));

        if (OceanCavesIndex != -1)
        {
            tasks.Insert(OceanCavesIndex + 1, new CoralReefGenPass("Coral Reef Gen", 100f));
        }
    }
}

public class CoralReefGenPass : GenPass
{
    public CoralReefGenPass(string name, float loadWeight) : base(name, loadWeight)
    {
    }

    private void PlaceRoundSplotch(ushort tileId, Point position, int size)
    {
        WorldUtils.Gen(position, new Shapes.Circle(size, size),
                       Actions.Chain(new Actions.ClearTile(), new Actions.SetTile(tileId)));
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        // t_* means tile coordinates, w_* means world cooridantes
        // Dig a square in the middle of the ocean down, with curved edges
        const int ReefOffset = 50;
        const int ReefDepth = 200;
        const int Depth = ReefDepth + ReefOffset;
        const int CurveAmount = 50;

        var t_edge_tiles = new List<Point>();

        var reefWidth = WorldGen.oceanDistance / 1.2f;
        var t_oceanX = Main.maxTilesX - reefWidth;
        var w_scanFrom = new Vector2(t_oceanX * 16, 0);
        var t_sandTile = TileHelper.FirstSolidFromTop(w_scanFrom, 512 * 16) ?? Point.Zero;

        // Mark as protected structure, because "Spreading Grass" step interferes with reef generation
        GenVars.structures.AddProtectedStructure(
            new Rectangle(t_sandTile.X, t_sandTile.Y + ReefOffset, (int)reefWidth, ReefDepth), 4);

        var round = new Animation<int>((int)reefWidth / 2, Ease.Out);
        var back_round = new Animation<int>((int)reefWidth / 2, Ease.In, [round]);

        for (int i = (int)t_oceanX; i < Main.maxTilesX; i++)
        {
            var round_rate = round.Animate(0, CurveAmount) ?? CurveAmount;
            var back_rate = back_round.Animate(0, CurveAmount) ?? 0;
            int last_j = 0;

            for (int j = t_sandTile.Y; j < t_sandTile.Y + (Depth - CurveAmount) + round_rate - back_rate; j++)
            {
                last_j = j;

                if (i == (int)t_oceanX && j > t_sandTile.Y + ReefOffset)
                {
                    t_edge_tiles.Add(new Point(i, j));
                }

                WorldGen.KillTile(i, j, noItem: true);
                Tile tile = Main.tile[i, j];
                tile.LiquidAmount = 255;
                tile.LiquidType = LiquidID.Water;
            }

            t_edge_tiles.Add(new Point(i, last_j));
        }

        // Place "coral" splotches along the edges
        var tendrilsAmount = WorldGen.genRand.Next(5, 10);
        var tendrilsCount = 0;
        var tendrilsPos = new List<int>();

        for (int i = 0; i < tendrilsAmount; i++)
        {
            tendrilsPos.Add(WorldGen.genRand.Next(t_sandTile.Y + ReefOffset, t_sandTile.Y + ReefOffset + ReefDepth));
        }

        foreach (var t_tile in t_edge_tiles)
        {
            // Place some sand on under the coral tiles
            var offset = new Point(0, 0);

            if (t_tile.X == (int)t_oceanX)
            {
                offset.X = -6;
            }
            else
            {
                offset.Y = 6;
            }

            PlaceRoundSplotch(TileID.Sandstone, new Point(t_tile.X + offset.X, t_tile.Y + offset.Y),
                              WorldGen.genRand.Next(3, 4));

            PlaceRoundSplotch(TileID.RedStucco, new Point(t_tile.X, t_tile.Y), WorldGen.genRand.Next(3, 4));

            // Randomly spawn tendrils from one side to the other
            if (t_tile.X == (int)t_oceanX && tendrilsPos.Contains(t_tile.Y))
            {
                tendrilsCount += 1;

                var angle = WorldGen.genRand.NextFloat(-0.2f, 0.2f);
                var direction = new Vector2(16, 0).RotatedBy(angle);
                var start = t_tile.ToWorldCoordinates();

                var iters = 0;
                var maxIters = 1000;

                var coralType =
                    WorldGen.genRand.NextFromList(TileID.RedStucco, TileID.YellowStucco, TileID.GreenStucco);

                while (((start + direction).X / 16) < Main.maxTilesX && iters < maxIters)
                {
                    iters++;

                    start += direction;
                    var adjust = WorldGen.genRand.NextFloat(-0.05f, 0.05f);
                    var current_angle = direction.ToRotation();

                    // Make sure it stays in the coral reef bounds
                    if (start.Y / 16 < t_sandTile.Y + ReefOffset)
                    {
                        adjust = MathF.Abs(adjust);
                    }
                    // And doesnt get too crazy with curving
                    else if (current_angle + adjust > MathHelper.PiOver4 ||
                             current_angle + adjust < -MathHelper.PiOver4)
                    {
                        adjust = 0;
                    }

                    direction = direction.RotatedBy(adjust);

                    PlaceRoundSplotch(coralType, new Point((int)(start.X / 16), (int)(start.Y / 16)),
                                      WorldGen.genRand.Next(2, 3));
                }
            }
        }
    }
}
