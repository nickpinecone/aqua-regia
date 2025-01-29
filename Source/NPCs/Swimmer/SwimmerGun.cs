using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Weapons;
using static AquaRegia.AquaRegia;

namespace AquaRegia.NPCs.Swimmer;

public class SwimmerGun : BaseWeapon
{
    public override string Texture => TexturesPath.NPCs + "Swimmer/SwimmerGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public SpriteModule Sprite { get; private set; }

    public SwimmerGun() : base()
    {
        Sound = new SoundModule();
        Property = new PropertyModule();
        Sprite = new SpriteModule();

        Composite.AddModule(Sound, Property, Sprite);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetProperties(this, 36, 24, 8, 0.8f, 3.2f, 16, 16, 22f, ItemRarityID.Blue);
        Property.SetProjectile<SwimmerProjectile>(this);

        Sprite.HoldoutOffset = new Vector2(-2f, 4f);
        Sprite.Offset = new Vector2(0f, 0f);
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        position = Sprite.ApplyOffset(position, velocity);
        velocity = Property.ApplyInaccuracy(velocity);
        var custom = new WeaponWithAmmoSource(source, this);

        Helper.SpawnProjectile<SwimmerProjectile>(custom, player, position, velocity, damage, knockback);

        return false;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }

    public override Vector2? HoldoutOffset()
    {
        return Sprite.HoldoutOffset;
    }
}
