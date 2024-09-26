using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Modules;
using AquaRegia.Utils;

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

    public override void RuntimeHitNPC(NPC target, NPC.HitInfo hit)
    {
        base.RuntimeHitNPC(target, hit);

        if (Main.rand.Next(0, 6) == 0)
        {
            target.AddBuff(BuffID.Frostburn, 240);
        }
    }
}
