using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Modules.Weapons;
using WaterGuns.Utils;
using WaterGuns.Sources;

namespace WaterGuns.Weapons.Sea;

public class SeaGun : BaseWeapon
{
    public override string Texture => TexturesPath.Weapons + "Sea/SeaGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }
    public SpriteModule Sprite { get; private set; }

    public SeaGun() : base()
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
        Property.SetProjectile<SeaProjectile>(this);

        Property.SetProperties(this, 58, 40, 12, 1.8f, 3.2f, 20, 20, 22f, ItemRarityID.Blue,
                               Item.sellPrice(0, 0, 80, 0));
        Sprite.SefDefaults(new Vector2(34f, 34f), new Vector2(-8, 2));

        Pump.SetDefaults(10);
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
            var direction = Main.MouseWorld - player.Center;
            direction.Normalize();
            var velocity = direction * 12f;

            Helper.SpawnProjectile<StarfishProjectile>(Item.GetSource_FromThis(), player, player.Center, velocity,
                                                       Item.damage, Item.knockBack);

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

        Helper.SpawnProjectile<SeaProjectile>(custom, player, position, velocity, damage, knockback);

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

    public override Vector2? HoldoutOffset()
    {
        return Sprite.HoldoutOffset;
    }
}
