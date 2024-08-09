using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Modules;
using WaterGuns.Modules.Weapons;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Golden;

public class GoldenGun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "Golden/GoldenGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }

    public GoldenGun() : base()
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
        Property.SetProjectile<GoldenProjectile>(this);

        Sprite.HoldoutOffset = new Vector2(-12, 6);
        Sprite.Offset = new Vector2(52f, 52f);
        Pump.MaxPumpLevel = 12;
        Property.Inaccuracy = 3.3f;

        Item.width = 58;
        Item.height = 40;
        Item.damage = 8;
        Item.knockBack = 1.2f;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.shootSpeed = 22f;

        Item.rare = ItemRarityID.White;
        Item.value = Item.sellPrice(0, 0, 20, 0);
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.DefaultUpdate();

        if (Main.mouseRight)
        {
            AltUseAlways(player);
        }
    }

    public void AltUseAlways(Player player)
    {
        if (Pump.Pumped)
        {
            Main.LocalPlayer.GetModPlayer<GoldenPlayer>().SpawnSword(player, (int)(Item.damage * 1.3f), Item.knockBack);

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

        ShootProjectile<GoldenProjectile>(player, source, position, velocity, damage, knockback);

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
}
