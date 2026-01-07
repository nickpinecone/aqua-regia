using AquaRegia.Library;
using AquaRegia.Library.Extended.Extensions;
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
    public override string Texture => Assets.Sprites.Ammo.bottled_cryogel;

    private PropertyModule Property { get; } = new();

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 50;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Set(this)
            .Defaults.BottledWater()
            .Damage(2, 0.2f)
            .Rarity(ItemRarityID.White)
            .Price(Item.sellPrice(copper: 8));
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

        var buff = new BuffModule();
        buff.SetDefaults(BuffID.Frostburn, 4.FromSeconds(), 15);

        projectile.Composite.AddRuntimeModule(buff);
    }
}