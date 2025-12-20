using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Ammo;
using AquaRegia.Library.Extended.Modules.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PropertyModule = AquaRegia.Library.Extended.Modules.Items.PropertyModule;

namespace AquaRegia.Ammo;

public class BottledCryogel : BaseAmmo
{
    public override string Texture => Assets.Ammo + nameof(BottledCryogel);

    private PropertyModule Property { get; }

    public BottledCryogel()
    {
        Property = new PropertyModule(this);

        Composite.AddModule(Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Damage(2, 0.2f, DamageClass.Ranged)
            .Ammo(ModContent.ItemType<BottledWater>())
            .Rarity(ItemRarityID.White)
            .MaxStack(Item.CommonMaxStack, true)
            .Price(Item.sellPrice(0, 0, 0, 8));
    }

    public override void AddRecipes()
    {
        CreateRecipe(25)
            .AddIngredient(ModContent.ItemType<BottledWater>(), 25)
            .AddIngredient(ItemID.IceBlock, 1)
            .Register();
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