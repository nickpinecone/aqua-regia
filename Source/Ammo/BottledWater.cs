using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AquaRegia.Ammo;

public class BottledWater : BaseAmmo
{
    public override string Texture => Assets.Sprites.Ammo.BottledWater;

    private PropertyModule Property { get; } = new();

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Set(this)
            .Damage(1, 0.1f, DamageClass.Ranged)
            .Ammo(ModContent.ItemType<BottledWater>())
            .Rarity(ItemRarityID.White)
            .MaxStack(Item.CommonMaxStack, true)
            .Price(Item.sellPrice(0, 0, 0, 5));
    }

    public override void AddRecipes()
    {
        CreateRecipe(25)
            .AddIngredient(ModContent.ItemType<EmptyBottle>(), 25)
            .AddCondition(Condition.NearWater)
            .Register();
    }
}