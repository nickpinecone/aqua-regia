using AquaRegia.Library.Extended.Data;
using AquaRegia.Library.Extended.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Items.SurfBoard;

public class SurfBoardPlayer : ModPlayer
{
    public bool IsBoarding { get; set; } = false;

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        base.ModifyDrawInfo(ref drawInfo);

        if (drawInfo.drawPlayer.HeldItem.ModItem is not SurfBoard surfBoard)
        {
            IsBoarding = false;
            return;
        }

        if (IsBoarding)
        {
            drawInfo.rotation = MathHelper.Clamp(Player.velocity.X * -0.05f, -0.1f, 0.1f);

            Player.SetLegFrame(PlayerFrames.Idle);
            Player.SetBodyFrame(PlayerFrames.Jump);
        }
    }
}