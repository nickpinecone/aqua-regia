using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Weapons;
using AquaRegia.Modules;

namespace AquaRegia.NPCs;

public class SwimmerGun : BaseGun
{
    public override string Texture => TexturesPath.NPCs + "SwimmerGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }

    public SwimmerGun() : base()
    {
        Sound = new SoundModule(this);
        Property = new PropertyModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetDefaults(this);
        Property.SetProjectile<SwimmerProjectile>(this);

        Sprite.HoldoutOffset = new Vector2(-2f, 4f);
        Sprite.Offset = new Vector2(0f, 0f);
        Property.Inaccuracy = 3.2f;

        Item.width = 36;
        Item.height = 24;
        Item.damage = 8;
        Item.knockBack = 0.8f;

        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.shootSpeed = 22f;

        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(0, 1, 0, 0);
    }

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            Item.noUseGraphic = true;
        }
        else
        {
            Item.noUseGraphic = false;
        }

        return true;
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        if (player.altFunctionUse == 2)
        {
            SpawnProjectile<PoolNoodle>(player, player.Center, Vector2.Zero, Item.damage * 2, Item.knockBack * 2f);
        }
        else
        {
            position = Sprite.ApplyOffset(position, velocity);
            velocity = Property.ApplyInaccuracy(velocity);

            ShootProjectile<SwimmerProjectile>(player, source, position, velocity, damage, knockback);
        }

        return false;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
