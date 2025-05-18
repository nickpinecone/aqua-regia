using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Utils;
using WaterGuns.Modules.Ammo;
using WaterGuns.Modules.Projectiles;

namespace WaterGuns.Ammo;

public class BottledCryogel : BaseAmmo
{
    public override string Texture => TexturesPath.Ammo + "BottledCryogel";

    public override void SetDefaults()
    {
        base.SetDefaults();

        SetProperties(2, 0.2f, ItemRarityID.White, Item.sellPrice(0, 0, 0, 8));
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

        if (projectile.Composite.TryGetModule(out WaterModule? water))
        {
            water.Color = Color.Cyan;
        }

        var frostburn = new BuffModule();
        frostburn.SetDefaults(BuffID.Frostburn, 4f, 15);

        projectile.Composite.AddRuntimeModule(frostburn);
    }
}
