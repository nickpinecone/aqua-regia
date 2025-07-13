using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Systems;

public class StaleWaterSystem : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_Liquid.Update += On_LiquidOnUpdate;
        On_Liquid.UpdateLiquid += On_LiquidOnUpdateLiquid;
        On_Liquid.SettleWaterAt += On_LiquidOnSettleWaterAt;
        On_Liquid.DelWater += On_LiquidOnDelWater;
    }

    public override void Unload()
    {
        base.Unload();

        On_Liquid.Update -= On_LiquidOnUpdate;
        On_Liquid.UpdateLiquid -= On_LiquidOnUpdateLiquid;
        On_Liquid.SettleWaterAt -= On_LiquidOnSettleWaterAt;
        On_Liquid.DelWater -= On_LiquidOnDelWater;
    }

    private void On_LiquidOnDelWater(On_Liquid.orig_DelWater orig, int l)
    {
    }

    private void On_LiquidOnSettleWaterAt(On_Liquid.orig_SettleWaterAt orig, int originX, int originY)
    {
    }

    private void On_LiquidOnUpdateLiquid(On_Liquid.orig_UpdateLiquid orig)
    {
    }

    private void On_LiquidOnUpdate(On_Liquid.orig_Update orig, Liquid self)
    {
    }
}
