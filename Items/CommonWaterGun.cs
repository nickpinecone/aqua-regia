
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace WaterGuns.Items
{
    public abstract class CommonWaterGun : ModItem
    {
        public int pumpLevel = 0;

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

        public override bool AltFunctionUse(Player player)
        {
            if (pumpLevel < 10)
                pumpLevel += 1;
            return base.AltFunctionUse(player);
        }

        public float CalculateAccuracy(float inaccuracy)
        {
            return Main.player[Main.myPlayer].GetModPlayer<GlobalPlayer>().CalculateAccuracy(inaccuracy);
        }

        public float CalculateSpeed()
        {
            return Main.player[Main.myPlayer].GetModPlayer<GlobalPlayer>().CalculateSpeed();
        }

        int hydropumpCount = 1;
        protected bool isOffset = true;
        protected float defaultInaccuracy = 1f;
        protected Vector2 offsetAmount = new Vector2(4, 4);
        protected Vector2 offsetIndependent = new Vector2(0, 0);
        public Projectile SpawnProjectile(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            WaterGuns.ProjectileData data = new WaterGuns.ProjectileData(source);

            // Mysterious mode
            if (source.Item.Name == "Mysterious Hydropump")
            {
                data.dustScale = 1.8f;
                data.dustAmount = 3;

                data.mysterious = hydropumpCount;
                if (hydropumpCount < 0)
                {
                    hydropumpCount = 2;
                }
                hydropumpCount -= 1;
            }

            // Crimson rainer color
            if (source.Item.Name == "Crimson Rainer")
            {
                data.color = new Color(255, 88, 61);
            }

            // Ammo Inflicts Statuses ------------------------------------------------------------
            if (source.AmmoItemIdUsed == ModContent.ItemType<Ammo.BottledWater.BottledBathWater>())
            {
                data.hasBuff = true;
                data.buffType = BuffID.Confused;
                data.buffTime = 240;

                data.color = new Color(247, 2, 248);
            }
            else if (source.AmmoItemIdUsed == ModContent.ItemType<Ammo.BottledWater.BottledIchor>())
            {
                data.hasBuff = true;
                data.buffType = BuffID.Ichor;
                data.buffTime = 240;

                data.color = new Color(255, 250, 41);
            }
            else if (source.AmmoItemIdUsed == ModContent.ItemType<Ammo.BottledWater.BottledVenom>())
            {
                data.hasBuff = true;
                data.buffType = BuffID.Venom;
                data.buffTime = 240;

                data.color = new Color(173, 103, 230);
            }
            else if (source.AmmoItemIdUsed == ModContent.ItemType<Ammo.BottledWater.BottledPoison>())
            {
                data.hasBuff = true;
                data.buffType = BuffID.Poisoned;
                data.buffTime = 240;

                data.color = new Color(0, 194, 129);
            }
            else if (source.AmmoItemIdUsed == ModContent.ItemType<Ammo.BottledWater.BottledCursedFire>())
            {
                data.hasBuff = true;
                data.buffType = BuffID.CursedInferno;
                data.buffTime = 240;

                data.color = new Color(96, 248, 2);
            }
            else if (source.AmmoItemIdUsed == ModContent.ItemType<Ammo.BottledWater.BottledCryogel>())
            {
                data.hasBuff = true;
                data.buffType = BuffID.Frostburn;
                data.buffTime = 240;

                data.color = new Color(67, 100, 176);
            }
            // -----------------------------------------------------------------------------------------

            // Ammo Special Effects
            else if (source.AmmoItemIdUsed == ModContent.ItemType<Ammo.BottledWater.BottledChlorophyte>())
            {
                data.homesIn = true;

                data.color = new Color(17, 143, 36);
            }
            else if (source.AmmoItemIdUsed == ModContent.ItemType<Ammo.BottledWater.BottledMeteorite>())
            {
                data.bounces = true;

                data.color = new Color(150, 56, 147);
            }

            float inaccuracy = CalculateAccuracy(defaultInaccuracy);
            // All of them use custom projectiles that shoot straight 
            // Make them a little inaccurate like in-game water gun
            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(inaccuracy)) * CalculateSpeed();
            // Offset if need be
            var offset = isOffset ? new Vector2(position.X + velocity.X * offsetAmount.X, position.Y + velocity.Y * offsetAmount.Y) : position;

            if (pumpLevel >= 10)
                data.fullCharge = true;
            damage += pumpLevel;
            knockback += pumpLevel / 5;
            data.dustScale += pumpLevel / 5;

            var proj = Projectile.NewProjectileDirect(data, offset + offsetIndependent, modifiedVelocity, type, damage, knockback, player.whoAmI);

            proj.scale += pumpLevel;
            proj.timeLeft += pumpLevel;

            if (pumpLevel > 0)
            {
                if (pumpLevel >= 10)
                    pumpLevel = 0;
                else
                {
                    pumpLevel -= 2;
                    if (pumpLevel < 0)
                        pumpLevel = 0;
                }
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
