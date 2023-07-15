
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;

namespace WaterGuns.Items
{
    public abstract class CommonWaterGun : ModItem
    {
        public int pumpLevel = 0;
        public int maxPumpLevel = 10;
        public bool increasePumpLevel = true;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterGun);
            Item.useAmmo = ItemID.BottledWater;

            SoundStyle WaterGunSoundStyle = new SoundStyle("WaterGuns/Sounds/WaterGunShoot");
            Item.UseSound = WaterGunSoundStyle with
            {
                Pitch = -0.1f,
                PitchVariance = 0.1f
            };
        }

        int pumpTimer = 0;
        public override void HoldItem(Player player)
        {
            if (increasePumpLevel)
            {
                pumpTimer += 1;
                if (pumpTimer >= 20)
                {
                    if (pumpLevel < maxPumpLevel)
                    {
                        pumpLevel += 1;
                    }

                    pumpTimer = 0;
                }
            }

            base.HoldItem(player);
        }

        int hydropumpCount = 1;
        protected bool isOffset = true;
        protected float defaultInaccuracy = 1f;
        protected Vector2 offsetAmount = new Vector2(4, 4);
        protected Vector2 offsetIndependent = new Vector2(0, 0);
        public Projectile SpawnProjectile(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool applyAffects = true)
        {
            WaterGuns.ProjectileData data = new WaterGuns.ProjectileData(source);

            // Apply ammo effects
            var ammo = (Ammo.BaseAmmo)ModContent.GetModItem(source.AmmoItemIdUsed);

            if (ammo != null)
            {
                damage += ammo.damage;
                data.hasBuff = ammo.hasBuff;
                data.buffType = ammo.buffType;
                data.buffTime = ammo.buffTime;
                data.color = ammo.color;

                data.homesIn = ammo.homesIn;
                data.bounces = ammo.bounces;
                data.spawnsStar = ammo.spawnsStar;
                data.penetrates = ammo.penetrates;
            }


            // Crimson rainer color
            if (source.Item.Name == "Crimson Rainer")
            {
                data.color = new Color(255, 88, 61);
                data.dustAmount = 4;
            }

            // All of them use custom projectiles that shoot straight 
            // Make them a little inaccurate like in-game water gun
            float inaccuracy = defaultInaccuracy;
            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(inaccuracy));

            if (pumpLevel >= maxPumpLevel && applyAffects)
            {
                inaccuracy = 0;
            }

            // Offset if need be
            var offset = isOffset ? new Vector2(position.X + velocity.X * offsetAmount.X, position.Y + velocity.Y * offsetAmount.Y) : position;

            if (pumpLevel >= maxPumpLevel)
            {
                data.fullCharge = true;

                if (applyAffects)
                {
                    damage *= 2;
                    knockback *= 1.5f;
                    data.dustScale *= 1.75f;
                }
            }

            // offsetIndependent changes projectile offset making them fire slighltly above the cursor
            // This rotates the velocity with a completly made up number
            modifiedVelocity = modifiedVelocity - offsetIndependent / 82;

            var proj = Projectile.NewProjectileDirect(data, offset + offsetIndependent, modifiedVelocity, type, damage, knockback, player.whoAmI);

            if (pumpLevel >= maxPumpLevel && applyAffects)
            {
                proj.scale *= 1.75f;
                proj.timeLeft = (int)(proj.timeLeft * 1.25f);
            }

            if (pumpLevel > 0 && increasePumpLevel)
            {
                if (pumpLevel >= maxPumpLevel)
                    pumpLevel = 0;
            }

            return proj;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SpawnProjectile(player, source, position, velocity, type, damage, knockback);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 4);
        }
    }
}
