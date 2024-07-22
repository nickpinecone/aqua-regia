using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using WaterGuns.Modules;
using WaterGuns.Modules.Weapons;

namespace WaterGuns.Weapons.Wooden;

public class TreeBoostModule : BaseGunModule
{
    private int _defaultDamage = 0;
    private int _boostAmount = 2;

    private List<ushort> _treeIds =
        new List<ushort>() { TileID.Trees,         TileID.PineTree,         TileID.PalmTree,
                             TileID.ChristmasTree, TileID.VanityTreeSakura, TileID.VanityTreeYellowWillow };

    public TreeBoostModule(BaseGun baseGun) : base(baseGun)
    {
    }

    public void Initialize(int defaultDamage, int boostAmount)
    {
        _defaultDamage = defaultDamage;
        _boostAmount = boostAmount;
    }

    public int Apply(Terraria.Player player)
    {
        bool isNearTree = false;
        foreach (var id in _treeIds)
        {
            if (player.IsTileTypeInInteractionRange(id, TileReachCheckSettings.Simple))
            {
                isNearTree = true;
                break;
            }
        }

        if (isNearTree)
        {
            return _defaultDamage + _boostAmount;
        }
        else
        {
            return _defaultDamage;
        }
    }
}
