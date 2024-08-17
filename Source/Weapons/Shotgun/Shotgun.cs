using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using AquaRegia.Modules;
using AquaRegia.Modules.Weapons;
using AquaRegia.Players;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Shotgun;

public class Shotgun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "Shotgun/Shotgun";

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
        Property.Inaccuracy = 16f;

        Item.width = 78;
        Item.height = 26;
        Item.damage = 14;
        Item.knockBack = 3f;

        Item.useTime = 38;
        Item.useAnimation = 38;
        Item.shootSpeed = 22f;

        Item.rare = ItemRarityID.Blue;
        Item.value = Item.buyPrice(0, 5, 25, 0);
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.DefaultUpdate();

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        if (Pump.Pumped && Main.LocalPlayer.GetModPlayer<ShotPlayer>().Chain == null)
        {
            var direction = Main.MouseWorld - player.Center;
            direction.Normalize();
            direction *= 24f;

            SpawnProjectile<ChainProjectile>(player, player.Center, direction, Item.damage, Item.knockBack);

            Pump.Reset();
        }
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        var spreads = new int[] { -2, -1, 1, 2 };
        var shotPlayer = Main.LocalPlayer.GetModPlayer<ShotPlayer>();

        foreach (var spread in spreads)
        {
            var positionCopy = Sprite.ApplyOffset(position, velocity);
            var velocityCopy = Property.ApplyInaccuracy(velocity);

            var up = velocity.RotatedBy(-MathHelper.PiOver2);
            up.Normalize();
            positionCopy += up * spread * Main.rand.NextFloat(2f, 3f);
            velocityCopy *= Main.rand.NextFloat(0.6f, 1f);

            if (shotPlayer.IsPulling)
            {
                var start = velocity.RotatedBy(-MathHelper.PiOver4);
                var end = velocity.RotatedBy(MathHelper.PiOver4);
                Particle.Arc(DustID.Smoke, positionCopy, new Vector2(8, 8), start, end, 3, 2f, 1.4f, 0, 55);

                ShootProjectile<WaterBalloon>(player, source, positionCopy, velocityCopy, damage, knockback);
            }
            else
            {
                ShootProjectile<ShotProjectile>(player, source, positionCopy, velocityCopy, damage, knockback);
            }
        }

        if (shotPlayer.IsPulling)
        {
            shotPlayer.IsPulling = false;

            SoundEngine.PlaySound(SoundID.Item36);
            Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(6, 4);

            var direction = Main.LocalPlayer.Center - Main.MouseWorld;
            direction.Normalize();
            var dist = (Main.LocalPlayer.Center - shotPlayer.Target.Center).Length();
            dist = (1 / dist) * Main.ViewSize.X;
            dist = MathHelper.Clamp(dist, 3f, 10f);
            direction *= dist;
            Main.LocalPlayer.velocity = direction;

            shotPlayer.Chain.Projectile.Kill();
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
