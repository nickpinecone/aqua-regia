using AquaRegia.Library;
using AquaRegia.Library.Extended.Extensions;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Items.SurfBoard;

public class SurfBoardDrawLayer : PlayerDrawLayer
{
    private Texture2D? _texture;

    public override void Load()
    {
        base.Load();

        _texture = ModContent.Request<Texture2D>(
            Assets.Items + nameof(SurfBoard),
            AssetRequestMode.ImmediateLoad
        ).Value;
    }

    public override Position GetDefaultPosition()
    {
        return PlayerDrawLayers.AfterLastVanillaLayer;
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (_texture == null) return;

        var boardPlayer = drawInfo.drawPlayer.GetModPlayer<SurfBoardPlayer>();

        if (drawInfo.drawPlayer.HeldItem.ModItem is not SurfBoard surfBoard)
        {
            boardPlayer.IsSurfing = false;
            return;
        }

        if (boardPlayer.IsSurfing || UnderwaterSystem.IsUnderwater(boardPlayer.Player.Center))
        {
            var position = ((boardPlayer.IsSurfing
                ? drawInfo.drawPlayer.Bottom
                : (drawInfo.drawPlayer.Top + new Vector2(0, -2))) - Main.screenPosition).ToVector2I();

            var drawData = new DrawData(
                _texture,
                position,
                null,
                Color.White,
                0f,
                _texture.Size() * 0.5f,
                1f,
                drawInfo.drawPlayer.ToHorizontalFlip(),
                0
            );

            if (boardPlayer.IsSurfing)
            {
                drawInfo.DrawDataCache.Insert(0, drawData);
            }
            else
            {
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }
}