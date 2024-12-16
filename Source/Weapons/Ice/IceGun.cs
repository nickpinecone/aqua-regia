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

        Property.SetProjectile<IceProjectile>(this);
        Pump.SetDefaults(16);

        Property.SetDefaults(this, 52, 26, 28, 3.0f, 1f, 18, 18, 22f, ItemRarityID.Green, Item.sellPrice(0, 8, 4, 0));
        Sprite.SefDefaults(new Vector2(46f, 46f), new Vector2(0, 6));

        Item.UseSound = SoundID.Item20;
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.DefaultUpdate();

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        var icePlayer = player.GetModPlayer<IcePlayer>();

        if (icePlayer.Bombs.Count > 0 && icePlayer.ReleasedRight)
        {
            icePlayer.ReleasedRight = false;

            foreach (var bomb in icePlayer.Bombs)
            {
                bomb.Explode();
            }

            icePlayer.ListenForRelease = true;
            icePlayer.Bombs.Clear();
        }

        else if (Pump.Pumped && icePlayer.ReleasedRight)
        {
            icePlayer.ReleasedRight = false;

            int startDelay = 0;

            for (int i = -2; i <= 1; i++)
            {
                var dir = Main.MouseWorld - player.Center;
                dir.Normalize();
                dir *= 8f;
                dir = dir.RotatedBy(Math.Sign(dir.X) * Main.rand.NextFloat(0.1f, 0.2f) * i);

                var source = new IceSource(Projectile.GetSource_NaturalSpawn());
                source.ExplodeDelay = startDelay;
                startDelay += 4;

                SpawnProjectile<FrozenBomb>(player, player.Center, dir, Item.damage, 0, source);
            }

            icePlayer.ListenForRelease = true;
            Pump.Reset();
        }
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {

        base.Shoot(player, source, position, velocity, type, damage, knockback);

        velocity = Property.ApplyInaccuracy(velocity);
        position = Sprite.ApplyOffset(position, velocity);

        ShootProjectile<IceProjectile>(player, source, position, velocity, damage, knockback);

        return false;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
