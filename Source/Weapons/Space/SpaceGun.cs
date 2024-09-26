using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Modules;
using AquaRegia.Modules.Weapons;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Space;

public class SpaceGun : BaseGun
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
        Property.SetProjectile<SpaceProjectile>(this);

        Property.SetDefaults(this, 76, 34, 22, 2.2f, 3.1f, 20, 20, 22f, ItemRarityID.Blue, Item.sellPrice(0, 2, 2, 0));
        Sprite.SefDefaults(new Vector2(42f, 42f), new Vector2(-20f, 4f));

        Pump.SetDefaults(16);
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
            SpawnProjectile<SpaceShip>(player, Main.MouseWorld, Vector2.Zero, Item.damage, 0);

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
