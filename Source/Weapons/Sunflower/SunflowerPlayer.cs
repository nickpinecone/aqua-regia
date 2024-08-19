using AquaRegia.Utils;
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

    public override void UpdateEquips()
    {
        if (Sunflower != null && Main.IsItDay())
        {
            Main.LocalPlayer.statDefense += 8;
            Main.LocalPlayer.GetDamage(DamageClass.Ranged) += 0.2f;
        }
    }
}
