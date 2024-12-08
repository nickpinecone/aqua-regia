using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Weapons;
using AquaRegia.Modules;

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

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        if (Pump.Pumped)
        {
            var dir = Main.MouseWorld - player.Center;
            dir.Normalize();
            dir *= 12f;

            SpawnProjectile<FrozenBomb>(player, player.Center, dir, Item.damage * 2, 0);

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

        ShootProjectile<FrostShard>(player, source, position, velocity, damage, knockback);

        return false;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
