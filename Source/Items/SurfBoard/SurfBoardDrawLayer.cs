using AquaRegia.Library;
using AquaRegia.Library.Extended.Extensions;
using AquaRegia.Library.Extended.Modules.Items;
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
        return PlayerDrawLayers.BeforeFirstVanillaLayer;
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var boardPlayer = drawInfo.drawPlayer.GetModPlayer<SurfBoardPlayer>();

        if (drawInfo.drawPlayer.HeldItem.ModItem is not SurfBoard surfBoard)
        {
            boardPlayer.IsBoarding = false;
            return;
        }

        if (boardPlayer.IsBoarding)
        {
            var texture = ModContent.Request<Texture2D>(
                Assets.Sprites + "SurfBoard",
                ReLogic.Content.AssetRequestMode.ImmediateLoad
            ).Value;

            var position = (drawInfo.drawPlayer.Bottom - Main.screenPosition).ToVector2I();

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

            drawInfo.DrawDataCache.Add(drawData);
        }
    }
}