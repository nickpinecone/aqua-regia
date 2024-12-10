using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Weapons;
using AquaRegia.Modules;
using System;

namespace AquaRegia.Weapons.Ice;

public class IceGun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "Ice/IceGun";

    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }

    public IceGun() : base()
    {
        Property = new PropertyModule(this);
        Pump = new PumpModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProjectile<FrostShard>(this);
        Pump.SetDefaults(2);

        Property.SetDefaults(this, 52, 26, 28, 3.0f, 1f, 16, 16, 32f, ItemRarityID.Green, Item.sellPrice(0, 8, 4, 0));
        Sprite.SefDefaults(new Vector2(26f, 26f), new Vector2(0, 6));

        Item.UseSound = SoundID.Item20;
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.DefaultUpdate();
    }

    public override bool AltFunctionUse(Player player)
    {
        base.AltFunctionUse(player);

        return true;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
                                          ref int damage, ref float knockback)
    {
        base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);

        if (player.altFunctionUse == 2)
        {
            var icePlayer = Main.LocalPlayer.GetModPlayer<IcePlayer>();
            var bomb = icePlayer.Bomb;

            if (bomb != null && !icePlayer.HasExploder && icePlayer.ReleasedRight)
            {
                var dir = bomb.Projectile.Center - player.Center;
                dir.Normalize();
                dir *= Item.shootSpeed * 1.5f;

                velocity = dir;
            }
        }
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {

        base.Shoot(player, source, position, velocity, type, damage, knockback);

        var icePlayer = Main.LocalPlayer.GetModPlayer<IcePlayer>();
        var bomb = icePlayer.Bomb;

        if (player.altFunctionUse == 2 && Pump.Pumped && bomb == null)
        {
            icePlayer.ListenForRelease = true;

            var dir = Main.MouseWorld - player.Center;
            dir.Normalize();
            dir *= 12f;

            SpawnProjectile<FrozenBomb>(player, player.Center, dir, Item.damage * 2, 0);

            Pump.Reset();
        }
        else if (player.altFunctionUse == 2 && (bomb != null && !icePlayer.HasExploder && icePlayer.ReleasedRight))
        {
            icePlayer.ReleasedRight = false;

            var iceSource = new IceSource(Projectile.GetSource_NaturalSpawn());
            iceSource.IsBombExploder = true;
            position = Sprite.ApplyOffset(position, velocity);
            SpawnProjectile<FrostShard>(player, position, velocity, 0, 0, iceSource);
        }
        else
        {
            velocity = Property.ApplyInaccuracy(velocity);
            position = Sprite.ApplyOffset(position, velocity);

            ShootProjectile<FrostShard>(player, source, position, velocity, damage, knockback);
        }

        return false;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
