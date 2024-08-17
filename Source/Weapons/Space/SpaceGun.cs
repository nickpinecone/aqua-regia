using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Modules;
using AquaRegia.Modules.Weapons;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Space;

public abstract class SpaceGun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "Space/SpaceGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }

    public SpaceGun() : base()
    {
        Sound = new SoundModule(this);
        Property = new PropertyModule(this);
        Pump = new PumpModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetDefaults(this);
        Property.SetProjectile<SpaceProjectile>(this);

        Sprite.HoldoutOffset = new Vector2(-20f, 4f);
        Sprite.Offset = new Vector2(42f, 42f);
        Pump.MaxPumpLevel = 16;
        Property.Inaccuracy = 3.1f;

        Item.width = 76;
        Item.height = 34;
        Item.damage = 22;
        Item.knockBack = 2.2f;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.shootSpeed = 22f;

        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(0, 2, 2, 0);
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.DefaultUpdate();

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        if (Pump.Pumped)
        {
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

        ShootProjectile<SpaceProjectile>(player, source, position, velocity, damage, knockback);

        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.Meteorite, 20);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
