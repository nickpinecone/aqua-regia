using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.Ice;

public class IcePlayer : ModPlayer
{
    private List<FrozenBomb> _bombs = new();

    public void AddBomb(FrozenBomb bomb)
    {
        _bombs.Add(bomb);
    }

    public bool BombCollide(Rectangle rect)
    {
        foreach (var bomb in _bombs)
        {
            if (bomb.WorldRectangle.Intersects(rect))
            {
                bomb.Explode();

                return true;
            }
        }

        return false;
    }

    public void RemoveBomb(FrozenBomb bomb)
    {
        _bombs.Remove(bomb);
    }
}
