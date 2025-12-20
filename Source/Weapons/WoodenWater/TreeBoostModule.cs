using System.Collections.Generic;
using System.Linq;
using AquaRegia.Library.Extended;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace AquaRegia.Weapons.WoodenWater;

public class TreeBoostModule : IModule
{
    private int _defaultDamage;
    private int _boostAmount;

    private readonly List<ushort> _treeIds =
    [
        TileID.Trees, TileID.PineTree, TileID.PalmTree,
        TileID.ChristmasTree, TileID.VanityTreeSakura, TileID.VanityTreeYellowWillow
    ];

    public void SetDefaults(int defaultDamage, int boostAmount)
    {
        _defaultDamage = defaultDamage;
        _boostAmount = boostAmount;
    }

    public int Apply(Player player)
    {
        var isNearTree = _treeIds.Any(id => player.IsTileTypeInInteractionRange(id, TileReachCheckSettings.Simple));

        return isNearTree
            ? _defaultDamage + _boostAmount
            : _defaultDamage;
    }
}