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

    public PropertyModule Property { get; }

    public SurfBoard()
    {
        // TODO dont know if we should have the parent on all modules, or just on ones that need it
        // i kinda like pure modules, but sometimes it's annoying
        Property = new PropertyModule(this);
        Composite.AddModule(Property);
    }

    public override void SetDefaults()
    {
        // TODO Kinda like that
        // Property
        //     .Size(28, 18)
        //     .Rarity(ItemRarityID.Blue);
        Item.width = 28;
        Item.height = 18;
        Item.maxStack = 1;
        Item.value = 100;
        Item.rare = ItemRarityID.Blue;

        SwimPlayer.PreUpdateMovementEvent += SwimPlayerOnPreUpdateMovementEvent;
        SwimPlayer.KeyHoldDownEvent += SwimPlayerOnKeyHoldDownEvent;
    }

    private void SwimPlayerOnKeyHoldDownEvent(SwimPlayer player, ref Vector2 velocity)
    {
        // Hijack vertical movement
        if (player.Player.HeldItem.ModItem is SurfBoard surfBoard)
        {
            // velocity = new Vector2(velocity.X, 0);
        }
    }

    // TODO So that works as well, kinda nice, no need to guess what executes where
    // only problem is, no way to access item's state
    private static void SwimPlayerOnPreUpdateMovementEvent(SwimPlayer player)
    {
        if (player.Player.HeldItem.ModItem is SurfBoard surfBoard)
        {
            // player.MaxSwimSpeed = 12f;
            player.SwimVelocity += new Vector2(0, -player.SwimSpeed);
        }
    }

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        // var swimPlayer = player.GetModPlayer<SwimPlayer>();
        //
        // // TODO need to think about this, right now im fried
        // if (UnderwaterSystem.IsUnderwater(player.Bottom))
        // {
        //     if (player.velocity.Y >= 0f)
        //         player.velocity = new Vector2(player.velocity.X, -player.gravity);
        //
        //     // TODO hooks on players GlobalItem event style
        //     player.maxRunSpeed *= 1.5f;
        //
        //     swimPlayer.SwimVelocity += new Vector2(0, -swimPlayer.SwimSpeed - 0.1f);
        //     swimPlayer.MaxSwimSpeed = 12f;
        // }
    }
}