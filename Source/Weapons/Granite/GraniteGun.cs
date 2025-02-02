using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Weapons;
using AquaRegia.Modules;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Weapons.Granite;

public class GraniteGun : BaseWeapon
{
    public override string Texture => TexturesPath.Weapons + "Granite/GraniteGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }
    public SpriteModule Sprite { get; private set; }

    public GraniteGun() : base()
    {
        Sound = new SoundModule();
        Property = new PropertyModule();
        Pump = new PumpModule();
        Sprite = new SpriteModule();

        Composite.AddModule(Sound, Property, Pump, Sprite);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetProjectile<GraniteProjecitle>(this);

        Property.SetProperties(this, 60, 26, 20, 2.4f, 3.1f, 26, 26, 22f, ItemRarityID.Blue, Item.sellPrice(0, 2, 0, 0));
        Sprite.SefDefaults(new Vector2(42f, 42f), new Vector2(-8f, 2f));

        Pump.SetDefaults(14);
    }

    public override bool CanUseItem(Player player)
    {
        return !player.GetModPlayer<GranitePlayer>().IsActive();
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
            player.GetModPlayer<GranitePlayer>().Activate(Item.damage);

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
        var custom = new WeaponWithAmmoSource(source, this);

        Helper.SpawnProjectile<GraniteProjecitle>(custom, player, position, velocity, damage, knockback);

        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe1 = CreateRecipe();
        recipe1.AddIngredient(ItemID.Granite, 20);
        recipe1.AddIngredient(ItemID.CrimtaneBar, 10);
        recipe1.AddTile(TileID.Anvils);
        recipe1.Register();

        Recipe recipe2 = CreateRecipe();
        recipe2.AddIngredient(ItemID.Granite, 20);
        recipe2.AddIngredient(ItemID.DemoniteBar, 10);
        recipe2.AddTile(TileID.Anvils);
        recipe2.Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }

    public override Vector2? HoldoutOffset()
    {
        return Sprite.HoldoutOffset;
    }
}
