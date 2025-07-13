using System;
using AquaRegia.Library.Helpers;
using AquaRegia.Library.Modules;
using AquaRegia.Library.Modules.Items;
using AquaRegia.Players;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AquaRegia.Items.SurfBoard;

public class SurfBoard : BaseItem
{
    public override string Texture => "AquaRegia/Assets/Sprites/SurfBoard";

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 18;
        Item.maxStack = 1;
        Item.value = 100;
        Item.rare = ItemRarityID.Blue;
    }

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        var swimPlayer = player.GetModPlayer<SwimPlayer>();

        // TODO need to think about this, right now im fried
        if (UnderwaterSystem.IsUnderwater(player.Bottom))
        {
            if(player.velocity.Y >= 0f)
                player.velocity = new Vector2(player.velocity.X, -player.gravity);
            
            // TODO hooks on players GlobalItem event style
            player.maxRunSpeed *= 1.5f;
            
            swimPlayer.SwimVelocity += new Vector2(0, -swimPlayer.SwimSpeed - 0.1f);
            swimPlayer.MaxSwimSpeed = 12f;
        }
    }
}