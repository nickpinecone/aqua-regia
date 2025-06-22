using System.Collections.Generic;
using AquaRegia.Library.Global;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AquaRegia.World;

// TODO Rename to underwater system
public class FloodSystem : ModSystem
{
    public static LocalizedText? FloodGenMessage { get; private set; }

    public override void SetStaticDefaults()
    {
        FloodGenMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"World.{nameof(FloodGenMessage)}"));
    }

    public override void Load()
    {
        base.Load();

        NpcGlobal.EditSpawnPoolEvent += EditSpawnPool;

        On_Liquid.Update += On_LiquidOnUpdate;
        On_Liquid.UpdateLiquid += On_LiquidOnUpdateLiquid;
        On_Liquid.SettleWaterAt += On_LiquidOnSettleWaterAt;
        On_Liquid.DelWater += On_LiquidOnDelWater;
    }

    private static void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        pool.Clear();
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

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        var finalCleanup = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));

        if (finalCleanup != -1)
        {
            tasks.Insert(finalCleanup + 1, new FloodGenPass("Flood", 100f));
        }
    }

    public override void PostUpdateEverything()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient) return;

        foreach (var player in Main.player)
        {
            if (!player.active) continue;

            var area = new Rectangle(
                (int)(player.position.X / 16) - 100,
                (int)(player.position.Y / 16) - 100,
                200,
                200
            );

            FloodArea(area);
        }
    }

    private void FloodArea(Rectangle area)
    {
        area = ClampToWorld(area);

        for (var x = area.Left; x < area.Right; x++)
        {
            for (var y = area.Top; y < area.Bottom; y++)
            {
                var tile = Main.tile[x, y];

                if (tile.LiquidAmount == 255) continue;

                tile.LiquidType = LiquidID.Water;
                tile.LiquidAmount = 255;

                WorldGen.SquareTileFrame(x, y, resetFrame: true);
            }
        }
    }

    private Rectangle ClampToWorld(Rectangle area)
    {
        area.X = (int)MathHelper.Clamp(area.X, 0, Main.maxTilesX);
        area.Y = (int)MathHelper.Clamp(area.Y, (int)WorldConstants.FloodLevel, Main.maxTilesY);
        return area;
    }
}

public class FloodGenPass : GenPass
{
    public FloodGenPass(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    private static void FloodWorld()
    {
        var left = 0;
        var right = Main.maxTilesX;
        var top = WorldConstants.FloodLevel;
        var bottom = Main.maxTilesY;

        for (var x = left; x < right; x++)
        {
            for (var y = (int)top; y < bottom; y++)
            {
                var tile = Main.tile[x, y];

                tile.LiquidType = LiquidID.Water;
                tile.LiquidAmount = 255;

                WorldGen.SquareTileFrame(x, y, resetFrame: true);
            }
        }
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = FloodSystem.FloodGenMessage?.Value;

        FloodWorld();
    }
}