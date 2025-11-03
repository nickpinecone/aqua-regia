using AquaRegia.Library.Extended.Data;
using AquaRegia.Library.Extended.Extensions;
using AquaRegia.Players;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Items.SurfBoard;

public class SurfBoardPlayer : ModPlayer
{
    public bool IsSurfing { get; set; } = false;

    public override void Load()
    {
        base.Load();

        SwimPlayer.CanModifyDrawInfoEvent += SwimPlayerOnCanModifyDrawInfoEvent;
    }

    public override void Unload()
    {
        base.Unload();

        SwimPlayer.CanModifyDrawInfoEvent -= SwimPlayerOnCanModifyDrawInfoEvent;
    }

    private static bool SwimPlayerOnCanModifyDrawInfoEvent(PlayerDrawSet drawInfo)
    {
        return drawInfo.drawPlayer.HeldItem.ModItem is not SurfBoard;
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        base.ModifyDrawInfo(ref drawInfo);

        if (drawInfo.drawPlayer.HeldItem.ModItem is not SurfBoard surfBoard)
        {
            IsSurfing = false;
            return;
        }

        if (IsSurfing || UnderwaterSystem.IsUnderwater(drawInfo.drawPlayer.Center))
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