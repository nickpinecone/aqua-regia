using AquaRegia.Library;
using AquaRegia.Library.Extended.Global;
using AquaRegia.Library.Extended.Helpers;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AquaRegia.Items.SurfBoard;

public class SurfBoard : BaseItem
{
    public override string Texture => Assets.Sprites.Items.SurfBoard;

    private PropertyModule Property { get; } = new();

    public override void Load()
    {
        base.Load();

        PlayerGlobal.PostUpdateRunSpeedsEvent += PlayerGlobalOnPostUpdateRunSpeedsEvent;
        PlayerGlobal.PreUpdateMovementEvent += PlayerGlobalOnPreUpdateMovementEvent;
    }

    public override void Unload()
    {
        base.Unload();

        PlayerGlobal.PostUpdateRunSpeedsEvent -= PlayerGlobalOnPostUpdateRunSpeedsEvent;
        PlayerGlobal.PreUpdateMovementEvent -= PlayerGlobalOnPreUpdateMovementEvent;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Set(this)
            .Size(28, 18)
            .MaxStack(1)
            .Price(Item.sellPrice(copper: 50))
            .Rarity(ItemRarityID.White);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Wood, 20)
            .AddTile(TileID.WorkBenches)
            .Register();
    }

    private static void PlayerGlobalOnPostUpdateRunSpeedsEvent(Player player)
    {
        var boardPlayer = player.GetModPlayer<SurfBoardPlayer>();

        if (player.HeldItem.ModItem is SurfBoard && boardPlayer.IsSurfing)
        {
            player.maxRunSpeed *= 1.25f;
        }
    }

    private static void PlayerGlobalOnPreUpdateMovementEvent(Player player)
    {
        if (player.HeldItem.ModItem is SurfBoard && player.wet && player.velocity.Y > -8f)
        {
            player.velocity += new Vector2(0, -0.3f);
        }
    }

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        var boardPlayer = player.GetModPlayer<SurfBoardPlayer>();
        boardPlayer.IsSurfing = false;

        if (
            TileHelper.IsWater(player.Bottom + new Vector2(0, 16)) &&
            !TileHelper.IsWater(player.Center) &&
            player.velocity.Y >= 0f && player.velocity.Y <= 4f
        )
        {
            boardPlayer.IsSurfing = true;
            player.Bottom = new Vector2(player.Bottom.X, (float)(player.Bottom.ToTileCoordinates().Y * 16) + 8);
            player.velocity = new Vector2(player.velocity.X, 0);
        }
    }
}