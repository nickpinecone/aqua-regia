using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace WaterGuns.UI;

class InterfaceSystem : ModSystem
{
    internal UserInterface _interface;
    internal PumpGauge _pumpGauge;
    private GameTime _lastUpdateUI;

    public override void Load()
    {
        base.Load();

        if (!Main.dedServ)
        {
            _interface = new UserInterface();
            _pumpGauge = new PumpGauge();
            _pumpGauge.Activate();
            _interface.SetState(_pumpGauge);
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        base.UpdateUI(gameTime);

        _lastUpdateUI = gameTime;
        if (_interface?.CurrentState != null)
        {
            _interface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        base.ModifyInterfaceLayers(layers);

        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("WaterGuns: Interface", delegate
            {
                if (_lastUpdateUI != null && _interface?.CurrentState != null)
                {
                    _interface.Draw(Main.spriteBatch, _lastUpdateUI);
                }
                return true;
            }, InterfaceScaleType.UI));
        }
    }

    public override void AddRecipeGroups()
    {
        base.AddRecipeGroups();

        RecipeGroup goldBars = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Bar",
                                               new int[] { ItemID.GoldBar, ItemID.PlatinumBar });
        RecipeGroup.RegisterGroup("WaterGuns:GoldBar", goldBars);

        RecipeGroup titaniumBars = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Titanium Bar",
                                                   new int[] { ItemID.TitaniumBar, ItemID.AdamantiteBar });
        RecipeGroup.RegisterGroup("WaterGuns:TitaniumBar", titaniumBars);
    }
}
