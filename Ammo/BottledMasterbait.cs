using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Ammo
{
    public class BottledMasterbait : BaseAmmo
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Poisons and charms your enemies");
            base.SetStaticDefaults();
        }
    }
}
