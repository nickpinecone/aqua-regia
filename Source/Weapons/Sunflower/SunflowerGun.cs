using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Weapons;
using AquaRegia.Modules;

namespace AquaRegia.Weapons.Sunflower;

public class SunflowerGun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "Sunflower/SunflowerGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }

    private Sunflower _sunflower = null;

    public SunflowerGun() : base()
    {
        Sound = new SoundModule(this);
        Property = new PropertyModule(this);
        Pump = new PumpModule(this, 20, 60);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetDefaults(this);
        Property.SetProjectile<SunflowerProjectile>(this);

        Sprite.HoldoutOffset = new Vector2(-12, 0);
        Sprite.Offset = new Vector2(48f, 48f);
        Pump.MaxPumpLevel = 20;
        Property.Inaccuracy = 14f;

        Item.width = 78;
        Item.height = 40;
        Item.damage = 16;
        Item.knockBack = 4f;

        Item.useTime = 38;
        Item.useAnimation = 38;
        Item.shootSpeed = 22f;

        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(0, 3, 0, 0);
    }

    public override void UpdateInventory(Player player)
    {
        base.UpdateInventory(player);

        if (_sunflower != null)
        {
            if (Main.IsItDay())
            {
                Pump.ColorA = Color.Gold;
                Pump.ColorB = Color.Yellow;
            }
            else
            {
                Pump.ColorA = Color.DarkRed;
                Pump.ColorB = Color.Red;
            }

            _sunflower.Projectile.timeLeft = 11;
            Pump.DefaultDecrease();

            if (Pump.PumpLevel == 0)
            {
                Pump.Reset();
                _sunflower = null;
            }
        }
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        if (_sunflower == null)
        {
            Pump.ColorA = Color.Blue;
            Pump.ColorB = Color.Cyan;

            Pump.DefaultUpdate();
        }

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        if (Pump.Pumped && _sunflower == null)
        {
            Pump.StartDecrease();
            _sunflower = SpawnProjectile<Sunflower>(player, player.Center, Vector2.Zero, Item.damage, Item.knockBack);
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
            velocityCopy *= Main.rand.NextFloat(0.7f, 1.1f);

            ShootProjectile<SunflowerProjectile>(player, source, positionCopy, velocityCopy, damage, knockback);
        }

        return false;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}