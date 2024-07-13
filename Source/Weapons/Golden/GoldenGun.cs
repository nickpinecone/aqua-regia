using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Players;
using WaterGuns.Projectiles.Golden;
using WaterGuns.Utils;
using WaterGuns.Weapons.Modules;

namespace WaterGuns.Weapons.Golden;

public class GoldenGun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "GoldenGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }

    private GoldenPlayer _goldenPlayer;

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
        Property.Inaccuracy = 3.2f;

        Item.width = 58;
        Item.height = 40;
        Item.damage = 12;
        Item.knockBack = 1.8f;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.shootSpeed = 22f;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        ChatLog.Message("Golden gun spawned");
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
            Main.LocalPlayer.GetModPlayer<GoldenPlayer>().SpawnSword(player, Item.damage, Item.knockBack);

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
        Recipe recipe = CreateRecipe();
        recipe.AddRecipeGroup("WaterGuns:GoldBar", 10);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
