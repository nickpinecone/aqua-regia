using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Ammo;

namespace WaterGuns.Modules;

public abstract class BaseAmmo : ModItem
{
    public Color AccentColor { get; set; }

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 25;
    }

    public override void SetDefaults()
    {
        Item.ammo = ModContent.ItemType<BottledWater>();
        Item.DamageType = DamageClass.Ranged;
        Item.maxStack = Item.CommonMaxStack;
        Item.consumable = true;

        Item.width = 14;
        Item.height = 28;

        Item.damage = 0;
        Item.knockBack = 0f;

        Item.rare = ItemRarityID.White;
        Item.value = Item.buyPrice(0, 0, 0, 5);
        AccentColor = Color.White;
    }

    public virtual void ApplyToProjectile(BaseProjectile baseProjectile)
    {
    }

    public virtual void RuntimeHitNPC(NPC target, NPC.HitInfo hit)
    {
    }

    public virtual void RuntimeKill()
    {
    }
}
