using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Utils;
using WaterGuns.Modules.Weapons;
using WaterGuns.Sources;
using Terraria.DataStructures;

namespace WaterGuns.Weapons.Wooden;

public class WoodenGun : BaseWeapon
{
    public override string Texture => TexturesPath.Weapons + "Wooden/WoodenGun";

    public SpriteModule Sprite { get; private set; }
    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }
    public TreeBoostModule TreeBoost { get; private set; }

    public WoodenGun() : base()
    {
        Sprite = new SpriteModule();
        Sound = new SoundModule();
        Property = new PropertyModule();
        Pump = new PumpModule();
        TreeBoost = new TreeBoostModule();

        Composite.AddModule(Sprite, Sound, Property, Pump, TreeBoost);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetProjectile<WoodenProjectile>(this);
        Pump.SetDefaults(8);

        Property.SetProperties(this, 38, 22, 4, 0.8f, 3.5f, 20, 20, 22f, ItemRarityID.White,
                               Item.sellPrice(0, 0, 0, 20));

        Sprite.SefDefaults(new Vector2(26f, 26f), new Vector2(0, 6));
        TreeBoost.SetDefaults(Item.damage, 2);
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.Update();
        Item.damage = TreeBoost.Apply(player);

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        if (Pump.Pumped)
        {
            var custom = new WeaponWithAmmoSource(Item.GetSource_FromThis(), this);

            Helper.SpawnProjectile<TreeProjectile>(custom, player, Main.MouseWorld, Vector2.Zero, Item.damage * 2,
                                                   Item.knockBack * 2);

            Pump.Reset();
        }
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        position = Sprite.ApplyOffset(position, velocity);
        velocity = Property.ApplyInaccuracy(velocity);
        var custom = new WeaponWithAmmoSource(source, this);

        Helper.SpawnProjectile<WoodenProjectile>(custom, player, position, velocity, damage, knockback);

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

    public override Vector2? HoldoutOffset()
    {
        base.HoldoutOffset();

        return Sprite.HoldoutOffset;
    }
}
