using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Ammo;

public class BottledCryogel : BaseAmmo
{
    public override string Texture => TexturesPath.Ammo + "BottledCryogel";

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.damage = 2;
        Item.knockBack = 0.2f;
        AccentColor = Color.Cyan;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(25);
        recipe.AddIngredient(ModContent.ItemType<BottledWater>(), 25);
        recipe.AddIngredient(ItemID.IceBlock, 1);
        recipe.Register();
    }

    public override void RuntimeHitNPC(NPC target, NPC.HitInfo hit)
    {
        base.RuntimeHitNPC(target, hit);

        if (Main.rand.Next(0, 6) == 0)
        {
            target.AddBuff(BuffID.Frostburn, 240);
        }
    }
}
