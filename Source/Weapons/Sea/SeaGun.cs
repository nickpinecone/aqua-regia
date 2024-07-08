using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Projectiles.Sea;
using WaterGuns.Utils;
using WaterGuns.Weapons.Modules;

namespace WaterGuns.Weapons.Sea;

public class SeaGun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "SeaGun";

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
        Property.SetDefaults(this);
        Property.SetProjectile<SeaProjectile>(this);

        Sprite.HoldoutOffset = new Vector2(-8, 2);
        Sprite.Offset = new Vector2(34f, 34f);
        Pump.MaxPumpLevel = 10;
        Property.Inaccuracy = 3.5f;

        Item.width = 58;
        Item.height = 40;
        Item.damage = 8;
        Item.knockBack = 1.2f;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.shootSpeed = 22f;
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.DefaultUpdate();

        if (Main.mouseRight)
        {
            Starfish(player);
        }
    }

    public void Starfish(Player player)
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
        recipe.AddIngredient(ItemID.Seashell, 10);
        recipe.AddIngredient(ItemID.Starfish, 8);
        recipe.AddIngredient(ItemID.Coral, 6);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
