using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Utils;
using WaterGuns.Modules.Weapons;
using WaterGuns.Modules;

namespace WaterGuns.NPC;

public class Swimmer_Gun : BaseGun
{
    public override string Texture => TexturesPath.NPC + "Simmer_Gun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }

    public Swimmer_Gun() : base()
    {
        Sound = new SoundModule(this);
        Property = new PropertyModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetDefaults(this);
        Property.SetProjectile<Swimmer_Projectile>(this);

        Sprite.HoldoutOffset = new Vector2(-2f, -1f);
        Sprite.Offset = new Vector2(3f, 3f);
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

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        position = Sprite.ApplyOffset(position, velocity);
        velocity = Property.ApplyInaccuracy(velocity);

        ShootProjectile<Swimmer_Projectile>(player, source, position, velocity, damage, knockback);

        return false;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
