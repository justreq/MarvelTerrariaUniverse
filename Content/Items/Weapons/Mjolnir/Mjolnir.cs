using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace MarvelTerrariaUniverse.Content.Items.Weapons.Mjolnir
{
    public class Mjolnir : ModItem
    {
        private int mode = 0;
        private bool charged = false;
        private float chargeInt = 0;

        //public override void SetStaticDefaults()
        //{
        //    ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        //}
        public override void SetDefaults()
        {

            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 44;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = 80000;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MjolnirLightning>();
            Item.useTurn = true;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            Item.channel = true;
            Item.shootSpeed = 5f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 0f);
        }

        public override bool CanUseItem(Player player)       //this make that you can shoot only 1 boomerang at once
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == ModContent.ProjectileType<MjolnirThrow>())
                {
                    return false;
                }
                else if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == ModContent.ProjectileType<MjolnirChargedThrow>())
                {
                    return false;
                }
            }
            return true;
        }
public override void HoldItem(Player player)
        {
            Item.useTime = 1;
            Item.useAnimation = 1;
            if (mode == 0)
            {
                Item.noUseGraphic = true;
                Item.shootSpeed = 10f;
                Item.shoot = ModContent.ProjectileType<MjolnirThrow>();
                if (charged)
                {
                    Item.shoot = ModContent.ProjectileType<MjolnirChargedThrow>();
                }
            }
            if (mode == 1)
            {
                Item.shootSpeed = 5f;
                Item.noUseGraphic = false;
            }
            if (mode == 2)
            {
                Item.noUseGraphic = false;
                Item.shoot = ModContent.ProjectileType<MjolnirFly>();
            }
            if (mode == 3)
            {
                Item.noUseGraphic = false;
                Item.shoot = ModContent.ProjectileType<MjolnirCharge>();
            }
        }   
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 1;
                Item.useAnimation = 1;
                if (mode < 3)
                {
                    mode++;
                }
                else
                {
                    mode = 0;
                }
                return false;
            }
            else if (mode == 0 && player.altFunctionUse != 2)
            {
                Item.useTime = 25;
                Item.useAnimation = 25;
            }
            else if (mode == 1 && player.altFunctionUse != 2)
            {
                if (!charged)
                {
                    chargeInt++;
                    Item.useTime = 1;
                    Item.useAnimation = 100;
                    position = new Vector2(player.Center.X, Main.screenPosition.Y);
                    Vector2 vector = new Vector2(velocity.X, velocity.Y);
                    Vector2 vector82 = -Main.player[Main.myPlayer].Center + Main.MouseWorld;
                    float ai = Main.rand.Next(1);
                    Vector2 vector83 = Vector2.Normalize(vector82) * Item.shootSpeed;
                    position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * -10f;
                    Vector2 vector2 = Vector2.Normalize(vector) * 25f;
                    if (Collision.CanHit(position, 0, 0, position + vector2, 0, 0))
                    {
                        position += vector2;
                    }
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MjolnirCharge>(), damage / 4, knockback, player.whoAmI, (int)DateTime.Now.Ticks);
                    if (chargeInt >= 100)
                    {
                        charged = true;
                    }
                    return false;
                }
                if (charged)
                {
                    Item.useTime = 1;
                    Item.useAnimation = 100;
                    Vector2 vector = new Vector2(velocity.X, velocity.Y);
                    Vector2 vector82 = -Main.player[Main.myPlayer].Center + Main.MouseWorld;
                    float ai = Main.rand.Next(1);
                    Vector2 vector83 = Vector2.Normalize(vector82) * Item.shootSpeed;
                    position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * -10f;
                    Vector2 vector2 = Vector2.Normalize(vector) * 25f;
                    if (Collision.CanHit(position, 0, 0, position + vector2, 0, 0))
                    {
                        position += vector2;
                    }
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MjolnirLightning>(), damage / 2, knockback, player.whoAmI, (int)DateTime.Now.Ticks);
                    chargeInt -= 0.25f;
                    if (chargeInt <= 0)
                    {
                        charged = false;
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
