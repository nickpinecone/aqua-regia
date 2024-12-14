using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.World.CoralReef.Tiles;

public class CoralTile : ModTile
{
    public override string Texture => TexturesPath.World + "CoralReef/Tiles/CoralTile";

    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;

        TileID.Sets.ChecksForMerge[Type] = true;
        Main.tileMerge[TileID.Sand][Type] = true;

        TileID.Sets.GeneralPlacementTiles[Type] = false;

        DustType = DustID.Coralstone;

        AddMapEntry(new Color(235, 114, 80));
    }

    public override void DrawEffects(int i, int j, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
                                     ref Terraria.DataStructures.TileDrawInfo drawData)
    {
        var top = new Point(i, j - 1);

        if (!TileHelper.IsSolid(top) && Main.rand.Next(0, 1000) == 0)
        {
            var position = new Point(i, j - 1).ToWorldCoordinates();
            var amount = Main.rand.Next(4, 8);
            var velocity = (-Vector2.UnitY * Main.rand.NextFloat(4f, 16f)).RotatedByRandom(0.4f);
            var scale = Main.rand.NextFloat(1f, 1.6f);
            var particle = Particle.Single(ParticleID.BreathBubble, position, new Vector2(4, 4), velocity, scale);
            particle.noGravity = true;
            particle.fadeIn = Main.rand.NextFloat(-12f, -24f);
        }
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        r = 0.5f;
        g = 0.5f;
        b = 0.5f;
    }

    public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right,
                                          ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
    {
        WorldGen.TileMergeAttempt(-2, TileID.Sand, ref up, ref down, ref left, ref right, ref upLeft, ref upRight,
                                  ref downLeft, ref downRight);
    }
}

public class CoralTileItem : ModItem
{
    public override string Texture => TexturesPath.World + "CoralReef/Tiles/CoralTileItem";

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.CoralstoneBlock);
        Item.createTile = ModContent.TileType<CoralTile>();
    }
}
