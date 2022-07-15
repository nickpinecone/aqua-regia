using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class IchorWaterAmmo : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("inflicts ichor debuff");
            base.SetStaticDefaults();
        }
    }
}
