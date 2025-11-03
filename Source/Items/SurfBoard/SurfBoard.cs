using AquaRegia.Library;
using AquaRegia.Library.Extended.Global;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;
using AquaRegia.Players;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AquaRegia.Items.SurfBoard;

public class SurfBoard : BaseItem
{
    public override string Texture => Assets.Sprites + "SurfBoard";

    public PropertyModule Property { get; }

    public SurfBoard()
    {
        Property = new PropertyModule(this);

        Composite.AddModule(Property);
    }

    public override void Load()
    {
        base.Load();

        PlayerGlobal.PostUpdateRunSpeedsEvent += PlayerGlobalOnPostUpdateRunSpeedsEvent;
        SwimPlayer.PreUpdateMovementEvent += SwimPlayerOnPreUpdateMovementEvent;
        SwimPlayer.KeyHoldDownEvent += SwimPlayerOnKeyHoldDownEvent;
    }

    public override void Unload()
    {
        base.Unload();

        PlayerGlobal.PostUpdateRunSpeedsEvent -= PlayerGlobalOnPostUpdateRunSpeedsEvent;
        SwimPlayer.PreUpdateMovementEvent -= SwimPlayerOnPreUpdateMovementEvent;
        SwimPlayer.KeyHoldDownEvent -= SwimPlayerOnKeyHoldDownEvent;
    }

    public override void SetDefaults()
    {
        Property
            .Size(28, 28)
            .MaxStack(1)
            .Price(Item.sellPrice(copper: 50))
            .Rarity(ItemRarityID.White);
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient(ItemID.Wood, 20)
            .AddTile(TileID.WorkBenches)
            .Register();
    }

    private void PlayerGlobalOnPostUpdateRunSpeedsEvent(Player player)
    {
        var boardPlayer = player.GetModPlayer<SurfBoardPlayer>();

        if (player.HeldItem.ModItem is SurfBoard surfBoard && boardPlayer.IsBoarding)
        {
            player.maxRunSpeed *= 1.25f;
        }
    }

    private void SwimPlayerOnKeyHoldDownEvent(SwimPlayer player, ref Vector2 velocity)
    {
        // Hijack vertical movement
        if (player.Player.HeldItem.ModItem is SurfBoard surfBoard)
        {
            velocity = new Vector2(velocity.X, 0);
        }
    }

    private static void SwimPlayerOnPreUpdateMovementEvent(SwimPlayer player)
    {
        if (player.Player.HeldItem.ModItem is SurfBoard surfBoard)
        {
            player.MaxSwimSpeed = 8f;
            player.SwimSpeedX = SwimPlayer.DefaultSwimSpeed / 4;
            player.SwimVelocity += new Vector2(0, -SwimPlayer.DefaultSwimSpeed);
        }
    }

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        var boardPlayer = player.GetModPlayer<SurfBoardPlayer>();
        boardPlayer.IsBoarding = false;

        if (
            UnderwaterSystem.IsUnderwater(player.Bottom) &&
            !UnderwaterSystem.IsUnderwater(player.Center) &&
            player.velocity.Y >= 0f && player.velocity.Y <= 1f
        )
        {
            boardPlayer.IsBoarding = true;
            player.Bottom = new Vector2(player.Bottom.X, (float)(UnderwaterSystem.TileSeaLevel * 16) + 8);
            player.velocity = new Vector2(player.velocity.X, 0);
        }
    }
}