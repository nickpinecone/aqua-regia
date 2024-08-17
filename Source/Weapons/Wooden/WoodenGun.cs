using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Weapons;
using AquaRegia.Modules;

namespace AquaRegia.Weapons.Wooden;

public class WoodenGun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "Wooden/WoodenGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }
    public TreeBoostModule TreeBoost { get; private set; }

    public WoodenGun() : base()
    {
        Sound = new SoundModule(this);
        Property = new PropertyModule(this);
        Pump = new PumpModule(this);
        TreeBoost = new TreeBoostModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetDefaults(this);
        Property.SetProjectile<WoodenProjectile>(this);

        Sprite.HoldoutOffset = new Vector2(0, 6);
        Sprite.Offset = new Vector2(26f, 26f);
        Pump.MaxPumpLevel = 8;
        Property.Inaccuracy = 3.5f;

        Item.width = 38;
        Item.height = 22;
        Item.damage = 4;
        Item.knockBack = 0.8f;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.shootSpeed = 22f;

        Item.rare = ItemRarityID.White;
        Item.value = Item.sellPrice(0, 0, 0, 20);

        TreeBoost.Initialize(Item.damage, 2);
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.DefaultUpdate();
        Item.damage = TreeBoost.Apply(player);

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        if (Pump.Pumped)
        {
            SpawnProjectile<TreeProjectile>(player, Main.MouseWorld, Vector2.Zero, Item.damage * 2, Item.knockBack * 2);

            Pump.Reset();
        }
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        position = Sprite.ApplyOffset(position, velocity);
        velocity = Property.ApplyInaccuracy(velocity);

        ShootProjectile<WoodenProjectile>(player, source, position, velocity, damage, knockback);

        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.Wood, 20);
        recipe.AddIngredient(ItemID.Acorn, 5);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
