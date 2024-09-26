using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Modules;
using AquaRegia.Modules.Weapons;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Sea;

public class SeaGun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "Sea/SeaGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }

    public SeaGun() : base()
    {
        Sound = new SoundModule(this);
        Property = new PropertyModule(this);
        Pump = new PumpModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetProjectile<SeaProjectile>(this);

        Property.SetDefaults(this, 58, 40, 12, 1.8f, 3.2f, 20, 20, 22f, ItemRarityID.Blue, Item.sellPrice(0, 0, 80, 0));
        Sprite.SefDefaults(new Vector2(34f, 34f), new Vector2(-8, 2));

        Pump.SetDefaults(10);
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
            var direction = Main.MouseWorld - player.Center;
            direction.Normalize();
            var velocity = direction * 12f;

            SpawnProjectile<StarfishProjectile>(player, player.Center, velocity, Item.damage, Item.knockBack);

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

        ShootProjectile<SeaProjectile>(player, source, position, velocity, damage, knockback);

        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.SlimeGun, 1);
        recipe.AddIngredient(ItemID.Seashell, 10);
        recipe.AddIngredient(ItemID.Starfish, 8);
        recipe.AddIngredient(ItemID.Coral, 6);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
