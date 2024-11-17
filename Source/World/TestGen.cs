using System;
using System.Collections.Generic;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using AquaRegia.World.CoralReef.Tiles;
using System.Linq;

namespace AquaRegia.World;

public class TestGen : ModSystem
{
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
        public bool Marked { get; set; }
        public EdgeType Type { get; set; }
    }

    private List<EdgeTile> _edgeTiles = new();

    public const int ReefOffset = 50;
    public const int ReefDepth = 200;
    public const int Depth = ReefDepth + ReefOffset;
    public const int MaxOffsetX = 200;

    public static bool JustPressed(Keys key)
    {
        return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
    }

    public override void PostUpdateWorld()
    {
        if (JustPressed(Keys.D9))
        {
            DebugInfo();
        }

        if (JustPressed(Keys.D0))
        {
            TestMethod((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
        }
    }

    private void DebugInfo()
    {
        ChatLog.Message(Main.LocalPlayer.Center.ToTileCoordinates().ToVector2(), "Player Center: ");
        ChatLog.Message(WorldGen.oceanDistance.ToString(), "Ocean Distance: ");
        ChatLog.Message(Main.maxTilesX.ToString(), "Max Tiles X: ");
        ChatLog.Message((Main.maxTilesX - WorldGen.oceanDistance).ToString(), "Max Tiles X - Ocean Distance: ");
        ChatLog.Message(WorldGen.oceanLevel.ToString(), "Ocean Level: ");
        var ocean = new OceanBiome();
        ChatLog.Message(ocean.IsInBiome(Main.LocalPlayer).ToString(), "Is In Ocean: ");
    }

    private void PlaceRoundSplotch(ushort tileId, Point position, int size)
    {
        WorldUtils.Gen(position, new Shapes.Circle(size, size),
                       Actions.Chain(new Actions.ClearTile(), new Actions.SetTile(tileId)));
    }

    private void TestMethod(int x, int y)
    {
        _edgeTiles.Clear();

        CreateOutline();

        // TODO to remove everything inside i first have to figure out the outer edges
        // Then using this i can go through every edge tile and remove everything to the right
        // If i come across an edge tile i mark that as hit
        // If a tile is hit, we do not remove tiles right to that
        // This way it removes everything inside of the edges area
        // Then we can make another pass on edges and place the coral tiles and sand
        // Probably save the edge tiles in a class, so we could mark them as hit,
        // save the angle that they were spawned as, so we can easily get a vector
        // perpendicular to that and place the sand splotches there
        // very easy = profit
    }

    private void CreateOutline()
    {
        // t_* means tile coordinates, w_* means world cooridantes
        // Dig a square in the middle of the ocean down, with curved edges
        var reefWidth = WorldGen.oceanDistance / 1.2f;
        var t_oceanX = Main.maxTilesX - reefWidth;
        var w_scanFrom = new Vector2(t_oceanX * 16, 0);
        var t_sandTile = TileHelper.FirstSolidFromTop(w_scanFrom, 512 * 16) ?? Point.Zero;

        // RIGHT SIDE
        var angle = WorldGen.genRand.NextFloat(0.05f, 0.1f);
        var direction = new Vector2(0, 16).RotatedBy(angle);
        var start = t_sandTile.ToWorldCoordinates();

        for (int j = (int)t_sandTile.Y; j < (int)t_sandTile.Y + Depth; j++)
        {
            start += direction;
            var adjust = WorldGen.genRand.NextFloat(-0.01f, 0.01f);
            var current_angle = direction.ToRotation();

            // Make sure it stays in the coral reef bounds
            if (start.X / 16 > t_sandTile.X)
            {
                adjust = 0.01f;
            }
            else if (start.X / 16 < t_sandTile.X - MaxOffsetX)
            {
                adjust = -0.01f;
            }
            // And doesnt get too crazy with curving
            if (current_angle + adjust > MathHelper.PiOver2 + MathHelper.PiOver4 ||
                current_angle + adjust < MathHelper.PiOver2 - MathHelper.PiOver4)
            {
                adjust = 0;
            }

            direction = direction.RotatedBy(adjust);

            _edgeTiles.Add(new EdgeTile()
            {
                X = (int)start.X / 16,
                Y = (int)start.Y / 16,
                DirectionCreated = direction,
                Marked = false,
                Type = EdgeType.Side,
            });

            //
            // PlaceRoundSplotch((ushort)ModContent.TileType<CoralTile>(), new Point((int)start.X / 16, (int)start.Y /
            // 16),
            //                   WorldGen.genRand.Next(3, 4));
        }

        // CORNER
        // angle = WorldGen.genRand.NextFloat(-0.1f, 0);
        // direction = new Vector2(0, 16).RotatedBy(angle);
        angle = direction.ToRotation();
        var diff = 0 - angle;
        var last = new Vector2(_edgeTiles.Last().X * 16, _edgeTiles.Last().Y * 16);
        start = last;
        var smooth = WorldGen.genRand.Next(64, 96);

        for (int i = 0; i < smooth; i++)
        {
            start += direction;
            var adjust = diff / smooth;

            direction = direction.RotatedBy(adjust);

            _edgeTiles.Add(new EdgeTile()
            {
                X = (int)start.X / 16,
                Y = (int)start.Y / 16,
                DirectionCreated = direction,
                Marked = false,
                Type = EdgeType.Corner,
            });
            // PlaceRoundSplotch((ushort)ModContent.TileType<CoralTile>(), new Point((int)start.X / 16, (int)start.Y /
            // 16),
            //                   WorldGen.genRand.Next(3, 4));
        }

        // BOTTOM SIDE
        angle = WorldGen.genRand.NextFloat(-0.1f, 0.1f);
        direction = new Vector2(16, 0).RotatedBy(angle);
        last = new Vector2(_edgeTiles.Last().X * 16, _edgeTiles.Last().Y * 16);
        start = last;

        for (int i = (int)last.X / 16; i < (int)Main.maxTilesX; i++)
        {
            start += direction;
            var adjust = WorldGen.genRand.NextFloat(-0.02f, 0.02f);
            var current_angle = direction.ToRotation();

            // Make sure it stays in the coral reef bounds
            if (start.Y / 16 < last.ToTileCoordinates().Y - MaxOffsetX / 2)
            {
                adjust = MathF.Abs(adjust);
            }

            // And doesnt get too crazy with curving
            if (current_angle + adjust > MathHelper.PiOver4 || current_angle + adjust < -MathHelper.PiOver4)
            {
                adjust = 0;
            }

            direction = direction.RotatedBy(adjust);

            _edgeTiles.Add(new EdgeTile()
            {
                X = (int)start.X / 16,
                Y = (int)start.Y / 16,
                DirectionCreated = direction,
                Marked = false,
                Type = EdgeType.Bottom,
            });

            // PlaceRoundSplotch((ushort)ModContent.TileType<CoralTile>(), new Point((int)start.X / 16, (int)start.Y /
            // 16),
            //                   WorldGen.genRand.Next(3, 4));
        }

        var lowest = _edgeTiles.MaxBy(e => e.Y);

        foreach (var edge in _edgeTiles)
        {
            if (edge.Y == lowest.Y)
            {
                continue;
            }

            var killedATile = false;

            if (!edge.Marked)
            {

                for (int i = edge.X; i < Main.maxTilesX; i++)
                {
                    if (_edgeTiles.Any(e => e.Y == edge.Y && edge.X == i && e != edge))
                    {
                        if (killedATile)
                        {
                            edge.Marked = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        killedATile = true;

                        WorldGen.KillTile(i, edge.Y, noItem: true);
                        Tile tile = Main.tile[i, edge.Y];
                        tile.LiquidAmount = 255;
                        tile.LiquidType = LiquidID.Water;
                    }
                }
            }
        }

        foreach (var edge in _edgeTiles)
        {
            // Place some sand on under the coral tiles
            var offset = edge.DirectionCreated.SafeNormalize(Vector2.Zero) * 6;
            offset = offset.RotatedBy(MathHelper.PiOver2);

            PlaceRoundSplotch(TileID.Sand, new Point(edge.X + (int)offset.X, edge.Y + (int)offset.Y),
                              WorldGen.genRand.Next(3, 4));

            if (edge.Y > t_sandTile.Y + ReefOffset)
            {
                PlaceRoundSplotch((ushort)ModContent.TileType<CoralTile>(), new Point(edge.X, edge.Y),
                                  WorldGen.genRand.Next(3, 4));
            }
            else
            {
                PlaceRoundSplotch(TileID.Sandstone, new Point(edge.X, edge.Y), WorldGen.genRand.Next(3, 4));
            }

            // PlaceRoundSplotch((ushort)ModContent.TileType<CoralTile>(), new Point(edge.X, edge.Y), 1);
            // WorldGen.PlaceTile(edge.X, edge.Y, ModContent.TileType<CoralTile>(), true, true);
        }

        var tendrilsAmount = WorldGen.genRand.Next(5, 10);
        var tendrilsPos = new List<EdgeTile>();

        var sideEdges = _edgeTiles.Where(e => e.Type == EdgeType.Side && e.Y > t_sandTile.Y + ReefOffset).ToList();

        for (int i = 0; i < tendrilsAmount; i++)
        {
            var count = sideEdges.Count();
            var randIndex = WorldGen.genRand.Next(0, count);
            var edge = sideEdges[randIndex];

            tendrilsPos.Add(edge);
        }

        for (int i = 0; i < tendrilsAmount; i++)
        {
            angle = WorldGen.genRand.NextFloat(-0.2f, 0.2f);
            direction = new Vector2(16, 0).RotatedBy(angle);
            start = new Vector2(tendrilsPos[i].X * 16, tendrilsPos[i].Y * 16);

            var iters = 0;
            var maxIters = 1000;

            var coralType = WorldGen.genRand.NextFromList(ModContent.TileType<CoralTile>());

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
                else if (current_angle + adjust > MathHelper.PiOver4 || current_angle + adjust < -MathHelper.PiOver4)
                {
                    adjust = 0;
                }

                direction = direction.RotatedBy(adjust);

                PlaceRoundSplotch((ushort)coralType, new Point((int)(start.X / 16), (int)(start.Y / 16)),
                                  WorldGen.genRand.Next(2, 3));
            }
        }
    }
}
