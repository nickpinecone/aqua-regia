using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.Sunflower;

public class SunflowerPlayer : ModPlayer
{
    public Sunflower Sunflower = null;

    public override void ModifyDrawInfo(ref Terraria.DataStructures.PlayerDrawSet drawInfo)
    {
        base.ModifyDrawInfo(ref drawInfo);

        if (Sunflower != null)
        {
            var position = Helper.ToVector2I(drawInfo.Center - Sunflower.Offset);

            Sunflower.Projectile.Center = position;
        }
    }
}
