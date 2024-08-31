using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Accessories.FrogHat;

public class FrogPlayer : ModPlayer
{
    public bool Active = false;
    public FrogMinion Minion = null;

    public override void DrawEffects(Terraria.DataStructures.PlayerDrawSet drawInfo, ref float r, ref float g,
                                     ref float b, ref float a, ref bool fullBright)
    {
        if (Minion != null)
        {
            var position = Helper.ToVector2I(drawInfo.Center + new Vector2(0, -16));

            Minion.Projectile.Bottom = position;
        }
    }
}
