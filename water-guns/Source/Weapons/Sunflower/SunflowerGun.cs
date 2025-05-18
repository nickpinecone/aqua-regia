using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Utils;
using WaterGuns.Modules.Weapons;
using WaterGuns.Modules;
using WaterGuns.Sources;
using WaterGuns.Global;
using System;
using Terraria.GameContent.ItemDropRules;

namespace WaterGuns.Weapons.Sunflower;

public class SunflowerGun : BaseWeapon
{
    public override string Texture => TexturesPath.Weapons + "Sunflower/SunflowerGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }
    public SpriteModule Sprite { get; private set; }

    static SunflowerGun()
    {
        NpcGlobal.ModifyNPCLootEvent += ModifyNPCLoot;
    }

    public SunflowerGun() : base()
    {
        Sound = new SoundModule();
        Property = new PropertyModule();
        Pump = new PumpModule();
        Sprite = new SpriteModule();

        Composite.AddModule(Sound, Property, Pump, Sprite);
    }

    private static void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.BloodZombie || npc.type == NPCID.Drippler)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new DownedEvilCondition(), ModContent.ItemType<SunflowerGun>(), 50));
        }
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetProjectile<SunflowerProjectile>(this);

        Property.SetProperties(this, 78, 40, 16, 4f, 14f, 38, 38, 22f, ItemRarityID.Blue, Item.sellPrice(0, 3, 0, 0));
        Sprite.SefDefaults(new Vector2(48f, 48f), new Vector2(-12, 0));

        Pump.SetDefaults(20);
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.Active = player.GetModPlayer<SunflowerPlayer>().Sunflower == null;

        Pump.Update();

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        if (Pump.Pumped && Pump.Active)
        {
            player.GetModPlayer<SunflowerPlayer>().SpawnSunflower(Item.damage, Item.knockBack);

            Pump.Reset();
        }
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        var spreads = new int[] { -2, -1, 1, 2 };
        var custom = new WeaponWithAmmoSource(source, this);

        foreach (var spread in spreads)
        {
            var positionCopy = Sprite.ApplyOffset(position, velocity);
            var velocityCopy = Property.ApplyInaccuracy(velocity);

            var up = velocity.RotatedBy(-MathHelper.PiOver2);
            up.Normalize();
            positionCopy += up * spread * Main.rand.NextFloat(2f, 3f);
            velocityCopy *= Main.rand.NextFloat(0.7f, 1.1f);

            Helper.SpawnProjectile<SunflowerProjectile>(custom, player, positionCopy, velocityCopy, damage, knockback);
        }

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
