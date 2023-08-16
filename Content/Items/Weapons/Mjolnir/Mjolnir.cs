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

        private enum ProjMode
        {
            Throwing,
            Lightning,
            Flying,
        }

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
            Item.shoot = ModContent.ProjectileType<MjolnirProjectile>();
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
            if (mode == (int)ProjMode.Throwing || mode == (int)ProjMode.Flying)
            {
                for (int i = 0; i < 1000; ++i)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == ModContent.ProjectileType<MjolnirProjectile>())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public override void HoldItem(Player player)
        {
            Item.useTime = 1;
            Item.useAnimation = 1;
            if (mode == (int)ProjMode.Throwing)
            {
                Item.noUseGraphic = true;
                Item.shootSpeed = 10f;
            }
            if (mode == (int)ProjMode.Lightning)
            {
                Item.shootSpeed = 5f;
                Item.noUseGraphic = false;
            }
            if (mode == (int)ProjMode.Flying)
            {
                Item.noUseGraphic = false;
            }
            //if (mode == 3)
            //{
            //    Item.noUseGraphic = false;
            //}
        }   
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (mode < 2) mode++;
                else mode = 0;

                return false;
            }
            else if (player.altFunctionUse != 2)
            {
                if (mode == (int)ProjMode.Throwing)
                {
                    Item.useTime = 25;
                    Item.useAnimation = 25;
                }
                else if (mode == (int)ProjMode.Lightning)
                {
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
                    if (!charged)
                    {
                        Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MjolnirProjectile>(), damage / 4, knockback, player.whoAmI, (int)DateTime.Now.Ticks, 1, 3);
                        chargeInt++;
                        if (chargeInt >= 100) charged = true;
                        return false;
                    }
                    else if (charged)
                    {
                        Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MjolnirProjectile>(), damage / 4, knockback, player.whoAmI, (int)DateTime.Now.Ticks, 2, 3);
                        chargeInt -= 0.25f;
                        if (chargeInt <= 0) charged = false;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
