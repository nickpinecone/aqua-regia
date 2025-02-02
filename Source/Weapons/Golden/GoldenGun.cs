using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Modules.Weapons;
using AquaRegia.Utils;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Weapons.Golden;

public class GoldenGun : BaseWeapon
{
    public override string Texture => TexturesPath.Weapons + "Golden/GoldenGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }
    public SpriteModule Sprite { get; private set; }

    public GoldenGun() : base()
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
        Property.SetProjectile<GoldenProjectile>(this);

        Property.SetProperties(this, 58, 40, 8, 1.2f, 3.3f, 20, 20, 22f, ItemRarityID.White,
                               Item.sellPrice(0, 0, 20, 0));
        Sprite.SefDefaults(new Vector2(52f, 52f), new Vector2(-12, 6));
        Pump.SetDefaults(12);
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
            player.GetModPlayer<GoldenPlayer>().SpawnSword(player, (int)(Item.damage * 1.3f), Item.knockBack);

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

        Helper.SpawnProjectile<GoldenProjectile>(custom, player, position, velocity, damage, knockback);

        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe1 = CreateRecipe();
        recipe1.AddIngredient(ItemID.GoldBar, 10);
        recipe1.AddTile(TileID.Anvils);
        recipe1.Register();

        Recipe recipe2 = CreateRecipe();
        recipe2.AddIngredient(ItemID.PlatinumBar, 10);
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
