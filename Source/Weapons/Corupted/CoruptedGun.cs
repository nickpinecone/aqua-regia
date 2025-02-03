using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Weapons;
using static AquaRegia.AquaRegia;
using Terraria.DataStructures;

namespace AquaRegia.Weapons.Corupted;

public class CoruptedGun : BaseWeapon
{
    public override string Texture => TexturesPath.Weapons + "Corupted/CoruptedGun";

    public SpriteModule Sprite { get; private set; }
    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }

    public CoruptedGun() : base()
    {
        Sprite = new SpriteModule();
        Sound = new SoundModule();
        Property = new PropertyModule();
        Pump = new PumpModule();

        Composite.AddModule(Sprite, Sound, Property, Pump);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetProjectile<CoruptedProjectile>(this);
        Pump.SetDefaults(8);

        Property.SetProperties(this, 80, 26, 14, 2f, 2.6f, 20, 20, 22f, ItemRarityID.Blue,
                               Item.sellPrice(0, 0, 8, 0));

        Sprite.SefDefaults(new Vector2(58f, 58f), new Vector2(-12, 3));
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.Update();

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        if (Pump.Pumped)
        {
            Pump.Reset();
        }

        Helper.SpawnProjectile<CoruptedWormHead>(Item.GetSource_FromThis(), player, Main.MouseWorld, Vector2.Zero, 10, 1f);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        position = Sprite.ApplyOffset(position, velocity);
        velocity = Property.ApplyInaccuracy(velocity);
        var weaponSource = new WeaponWithAmmoSource(source, this);
        var custom = new CoruptedSource(weaponSource, 0);

        Helper.SpawnProjectile<CoruptedProjectile>(custom, player, position, velocity, damage, knockback);

        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.ShadowScale, 5);
        recipe.AddIngredient(ItemID.DemoniteBar, 10);
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
