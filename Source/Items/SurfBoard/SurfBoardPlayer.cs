using AquaRegia.Library.Extended.Data;
using AquaRegia.Library.Extended.Extensions;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Items.SurfBoard;

public class SurfBoardPlayer : ModPlayer
{
    public bool IsSurfing { get; set; } = false;

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        base.ModifyDrawInfo(ref drawInfo);

        if (drawInfo.drawPlayer.HeldItem.ModItem is not SurfBoard)
        {
            IsSurfing = false;
            return;
        }

        if (IsSurfing || drawInfo.drawPlayer.wet)
        {
            drawInfo.rotation = MathHelper.Clamp(Player.velocity.X * -0.05f, -0.1f, 0.1f);

            Player.SetLegFrame(PlayerFrames.Idle);

            if (!IsSurfing)
            {
                Player.SetBodyFrame(PlayerFrames.Jump);
            }
        }
    }
}