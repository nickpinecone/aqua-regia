using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Ammo;
using AquaRegia.Modules.Projectiles;

namespace AquaRegia.Ammo;

public class BottledCryogel : BaseAmmo
{
    public override string Texture => TexturesPath.Ammo + "BottledCryogel";

    public override void SetDefaults()
    {
        base.SetDefaults();

        SetProperties(2, 0.2f, ItemRarityID.White, Item.sellPrice(0, 0, 0, 8), Color.Cyan);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(25);
        recipe.AddIngredient(ModContent.ItemType<BottledWater>(), 25);
        recipe.AddIngredient(ItemID.IceBlock, 1);
        recipe.Register();
    }

    public override void ApplyToProjectile(BaseProjectile projectile)
    {
        base.ApplyToProjectile(projectile);

        var frostburn = new BuffModule();
        frostburn.SetDefaults(BuffID.Frostburn, 4f, 15);

        projectile.Composite.AddRuntimeModule(frostburn);
    }
}
