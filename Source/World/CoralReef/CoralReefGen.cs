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
using AquaRegia.World.CoralReef.Tiles;
using System.Linq;
using AquaRegia.Config;

namespace AquaRegia.World.CoralReef;

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
        CoralTileCount = tileCounts[ModContent.TileType<CoralTile>()];
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

        int FinalCleanup = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));

        if (FinalCleanup != -1)
        {
            tasks.Insert(FinalCleanup, new CoralCleanupPass("Coral Cleanup", 100f));
        }
    }
}

public class CoralReefGenPass : GenPass
{
    public CoralReefGenPass(string name, float loadWeight) : base(name, loadWeight)
    {
    }

    private enum EdgeType
    {
        Side,
        Corner,
        Bottom,
    }

    private class EdgeTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 DirectionCreated { get; set; }
        public EdgeType Type { get; set; }

        public Vector2 ToWorldCoordinates()
        {
            return new Vector2(X * 16, Y * 16);
        }
    }

    private List<EdgeTile> t_edgeTiles = new();

    public const int ReefOffset = 50;
    public const int ReefDepth = 200;
    public const int Depth = ReefDepth + ReefOffset;

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        GenerateOutline();
        ClearInside();
        PlaceOutline();
        GenerateTendrils();

        var first = t_edgeTiles.First();
        var left = t_edgeTiles.MinBy(e => e.X);
        var bottom = t_edgeTiles.MaxBy(e => e.Y);
        var right = t_edgeTiles.MaxBy(e => e.X);
        var rect = new Rectangle(left.X, first.Y, right.X - left.X, bottom.Y - first.Y);
        GenVars.structures.AddProtectedStructure(rect, 4);
    }

    private void GenerateOutline()
    {
        var t_reefWidth = WorldGen.oceanDistance / 1.2f;
        var t_oceanX = Main.maxTilesX - t_reefWidth;
        var w_scanFrom = new Vector2(t_oceanX * 16, 0);
        var t_sandTile = TileHelper.FirstSolidFromTop(w_scanFrom, 512 * 16) ?? Point.Zero;

        t_edgeTiles.Add(new EdgeTile() {
            X = t_sandTile.X,
            Y = t_sandTile.Y,
            DirectionCreated = Vector2.UnitY,
            Type = EdgeType.Side,
        });

        var angle = WorldGen.genRand.NextFloat(0.05f, 0.1f);
        var direction = new Vector2(0, 16).RotatedBy(angle);
        var start = t_sandTile.ToWorldCoordinates();

        // Side
        for (int j = (int)t_sandTile.Y; j < (int)t_sandTile.Y + Depth; j++)
        {
            start += direction;

            t_edgeTiles.Add(new EdgeTile() {
                X = (int)start.X / 16,
                Y = (int)start.Y / 16,
                DirectionCreated = direction,
                Type = EdgeType.Side,
            });

            var adjust = WorldGen.genRand.NextFloat(-0.01f, 0.01f);
            var currentAngle = direction.ToRotation();

            // Make sure it stays in the bounds
            if (currentAngle + adjust < 0.04f + MathHelper.PiOver2)
            {
                adjust = 0.01f;
            }

            direction = direction.RotatedBy(adjust);
        }

        // Corner
        angle = direction.ToRotation();
        var diff = 0 - angle;
        start = t_edgeTiles.Last().ToWorldCoordinates();
        var smooth = WorldGen.genRand.Next(64, 96);

        for (int i = 0; i < smooth; i++)
        {
            start += direction;

            t_edgeTiles.Add(new EdgeTile() {
                X = (int)start.X / 16,
                Y = (int)start.Y / 16,
                DirectionCreated = direction,
                Type = EdgeType.Corner,
            });

            var adjust = diff / smooth;
            direction = direction.RotatedBy(adjust);
        }

        // Bottom
        direction = new Vector2(16, 0);
        start = t_edgeTiles.Last().ToWorldCoordinates();

        for (int i = (int)start.X / 16; i < (int)Main.maxTilesX; i++)
        {
            start += direction;

            t_edgeTiles.Add(new EdgeTile() {
                X = (int)start.X / 16,
                Y = (int)start.Y / 16,
                DirectionCreated = direction,
                Type = EdgeType.Bottom,
            });

            var adjust = WorldGen.genRand.NextFloat(-0.01f, 0.01f);
            var currentAngle = direction.ToRotation();

            // Make sure it stays in bounds
            if (currentAngle + adjust > 0.1f)
            {
                adjust = -0.01f;
            }
            else if (currentAngle + adjust < -0.1f)
            {
                adjust = 0.01f;
            }

            direction = direction.RotatedBy(adjust);
        }
    }

    private void PlaceWater(int x, int y)
    {
        WorldGen.KillTile(x, y, noItem: true);
        WorldGen.KillWall(x, y);
        Tile tile = Main.tile[x, y];
        tile.LiquidAmount = 255;
        tile.LiquidType = LiquidID.Water;
    }

    private void ClearInside()
    {
        foreach (var edge in t_edgeTiles)
        {
            if (edge.Type == EdgeType.Side || edge.Type == EdgeType.Corner)
            {
                for (int i = edge.X; i < Main.maxTilesX; i++)
                {
                    if (t_edgeTiles.Any(e => e.Y == edge.Y && edge.X == i && e != edge))
                    {
                        continue;
                    }

                    PlaceWater(i, edge.Y);
                }
            }
            else
            {
                var firstEdge = t_edgeTiles.First();

                for (int j = edge.Y; j > firstEdge.Y; j--)
                {
                    if (t_edgeTiles.Any(e => e.X == edge.X && edge.Y == j && e != edge))
                    {
                        continue;
                    }

                    PlaceWater(edge.X, j);
                }
            }
        }
    }

    private void PlaceRoundSplotch(ushort tileId, Point position, int size)
    {
        WorldUtils.Gen(position, new Shapes.Circle(size, size),
                       Actions.Chain(new Actions.ClearTile(), new Actions.ClearWall(), new Actions.SetTile(tileId)));
    }

    private void PlaceOutline()
    {
        foreach (var edge in t_edgeTiles)
        {
            // Place some sand on under the coral tiles
            var offset = edge.DirectionCreated.SafeNormalize(Vector2.Zero) * 6;
            offset = offset.RotatedBy(MathHelper.PiOver2);

            PlaceRoundSplotch(TileID.Sand, new Point(edge.X + (int)offset.X, edge.Y + (int)offset.Y),
                              WorldGen.genRand.Next(3, 4));

            var firstEdge = t_edgeTiles.First();

            if (edge.Y > firstEdge.Y + ReefOffset)
            {
                PlaceRoundSplotch((ushort)ModContent.TileType<CoralTile>(), new Point(edge.X, edge.Y),
                                  WorldGen.genRand.Next(3, 4));
            }
            else
            {
                PlaceRoundSplotch(TileID.Sand, new Point(edge.X, edge.Y), WorldGen.genRand.Next(3, 4));
            }
        }
    }

    private void GenerateTendrils()
    {
        var tendrilsAmount = WorldGen.genRand.Next(5, 10);
        var tendrilsPos = new List<EdgeTile>();

        var firstEdge = t_edgeTiles.First();
        var sideEdges =
            t_edgeTiles
                .Where(e => (e.Type == EdgeType.Side || e.Type == EdgeType.Corner) && e.Y > firstEdge.Y + ReefOffset)
                .ToList();

        for (int i = 0; i < tendrilsAmount; i++)
        {
            var count = sideEdges.Count();
            var randIndex = WorldGen.genRand.Next(0, count);
            var edge = sideEdges[randIndex];

            tendrilsPos.Add(edge);
        }

        for (int i = 0; i < tendrilsAmount; i++)
        {
            var angle = WorldGen.genRand.NextFloat(-0.2f, 0.2f);
            var direction = new Vector2(16, 0).RotatedBy(angle);
            var start = new Vector2(tendrilsPos[i].X * 16, tendrilsPos[i].Y * 16);

            var iters = 0;
            var maxIters = 1000;

            var coralType = WorldGen.genRand.NextFromList(ModContent.TileType<CoralTile>());
            var highestBottom = t_edgeTiles.Where(e => e.Type == EdgeType.Bottom).MinBy(e => e.Y);

            while (((start + direction).X / 16) < Main.maxTilesX && iters < maxIters)
            {
                iters++;

                start += direction;
                var adjust = WorldGen.genRand.NextFloat(-0.05f, 0.05f);
                var currentAngle = direction.ToRotation();

                // Make sure it stays in the coral reef bounds
                if (start.Y / 16 < firstEdge.Y + ReefOffset)
                {
                    adjust = 0.02f;
                }
                else if (start.Y / 16 > highestBottom.Y)
                {
                    adjust = -0.04f;
                }
                // And doesnt get too crazy with curving
                else if (currentAngle + adjust > MathHelper.PiOver4)
                {
                    adjust = -0.02f;
                }
                else if (currentAngle + adjust < -MathHelper.PiOver4)
                {
                    adjust = 0.02f;
                }

                direction = direction.RotatedBy(adjust);

                PlaceRoundSplotch((ushort)coralType, new Point((int)(start.X / 16), (int)(start.Y / 16)),
                                  WorldGen.genRand.Next(2, 3));
            }
        }
    }
}
