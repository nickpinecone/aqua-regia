using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AquaRegia.Ammo;

namespace AquaRegia.Modules;

public abstract class BaseAmmo : ModItem
{
    public Color AccentColor { get; set; }

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 25;
    }

    public override void SetDefaults()
    {
    }

    protected void SetProperties(int damage = 0, float knockBack = 0f, int rarity = 0, int sellPrice = 0,
                               Color? accentColor = null, int width = 14, int height = 28, int? maxStack = null,
                               bool consumable = true, int? ammo = null, DamageClass damageType = null)
    {
        Item.damage = damage;
        Item.knockBack = knockBack;

        Item.rare = rarity;
        Item.value = sellPrice;
        AccentColor = accentColor ?? Color.White;

        Item.width = width;
        Item.height = height;

        Item.consumable = consumable;
        Item.ammo = ammo ?? ModContent.ItemType<BottledWater>();
        Item.DamageType = damageType ?? DamageClass.Ranged;
        Item.maxStack = maxStack ?? Item.CommonMaxStack;
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
