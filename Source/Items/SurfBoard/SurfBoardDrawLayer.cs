using AquaRegia.Library;
using AquaRegia.Library.Extended.Extensions;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Items.SurfBoard;

public class SurfBoardDrawLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition()
    {
        return PlayerDrawLayers.AfterLastVanillaLayer;
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var boardPlayer = drawInfo.drawPlayer.GetModPlayer<SurfBoardPlayer>();

        if (drawInfo.drawPlayer.HeldItem.ModItem is not SurfBoard surfBoard)
        {
            boardPlayer.IsSurfing = false;
            return;
        }

        if (boardPlayer.IsSurfing || UnderwaterSystem.IsUnderwater(boardPlayer.Player.Center))
        {
            var texture = ModContent.Request<Texture2D>(
                Assets.Items + nameof(SurfBoard),
                ReLogic.Content.AssetRequestMode.ImmediateLoad
            ).Value;

            var position = ((boardPlayer.IsSurfing
                ? drawInfo.drawPlayer.Bottom
                : (drawInfo.drawPlayer.Top + new Vector2(0, -2))) - Main.screenPosition).ToVector2I();

            var drawData = new DrawData(
                texture,
                position,
                null,
                Color.White,
                0f,
                texture.Size() * 0.5f,
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