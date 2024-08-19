using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Accessories.FrogHat;

public class FrogPlayer : ModPlayer
{
    public bool Active = false;
    public FrogMinion Minion = null;

    public override void ModifyDrawInfo(ref Terraria.DataStructures.PlayerDrawSet drawInfo)
    {
        base.ModifyDrawInfo(ref drawInfo);

        if (Minion != null)
        {
            var position = Helper.ToVector2I(drawInfo.Center + new Vector2(0, -16));

            Minion.Projectile.Bottom = position;
        }
    }
}
