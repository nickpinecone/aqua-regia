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
        base.DrawEffects(i, j, spriteBatch, ref drawData);

        Main.LocalPlayer.GetModPlayer<CoralPlayer>().AddBreathSpot(new Point(i, j));
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
