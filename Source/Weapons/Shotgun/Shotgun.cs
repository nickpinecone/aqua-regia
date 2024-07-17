using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using WaterGuns.Projectiles.Shotgun;
using WaterGuns.Utils;
using WaterGuns.Weapons.Modules;

namespace WaterGuns.Weapons.Shotgun;

public class Shotgun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "Shotgun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }

    public Shotgun() : base()
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
        Property.SetProjectile<ShotProjectile>(this);

        Sprite.HoldoutOffset = new Vector2(-12, 0);
        Sprite.Offset = new Vector2(56f, 56f);
        Pump.MaxPumpLevel = 10;
        Property.Inaccuracy = 12f;

        Item.width = 78;
        Item.height = 26;
        Item.damage = 20;
        Item.knockBack = 3f;

        Item.useTime = 32;
        Item.useAnimation = 32;
        Item.shootSpeed = 22f;
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
            Pump.Reset();
        }
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        var spreads = new int[] { -2, -1, 1, 2 };

        foreach (var spread in spreads)
        {
            var positionCopy = Sprite.ApplyOffset(position, velocity);
            var velocityCopy = Property.ApplyInaccuracy(velocity);

            var up = velocity.RotatedBy(-MathHelper.PiOver2);
            up.Normalize();
            positionCopy += up * spread * Main.rand.NextFloat(2f, 3f);

            var shot = ShootProjectile<ShotProjectile>(player, source, positionCopy, velocityCopy, damage, knockback);
            shot.Projectile.timeLeft += Main.rand.Next(-5, 5);
        }

        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddCondition(LocalizedText.Empty, () => false);
        recipe.Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
