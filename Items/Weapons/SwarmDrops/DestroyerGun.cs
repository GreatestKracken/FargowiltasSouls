﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class DestroyerGun : ModItem
    {
        int shootNum = 0;
        int head;
        int current;
        int previous = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Destroyer Gun");
            Tooltip.SetDefault("''");
        }

        public override void SetDefaults()
        {
            item.damage = 45;
            item.summon = true;
            item.width = 24;
            item.height = 24;
            item.useTime = 5;
            item.useAnimation = 50;
            item.reuseDelay = 20;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1.5f;
            item.UseSound = new LegacySoundStyle(4, 13);
            item.value = 50000;
            item.rare = 5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("DestroyerHead");
            item.shootSpeed = 10f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //shoot head
            if (shootNum == 0 || player.ownedProjectileCounts[mod.ProjectileType("DestroyerHead")] == 0)
            {
                current = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
                head = current;

                shootNum++;
            }
            //shoot tail
            else if (shootNum == 9)
            {
                current = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("DestroyerTail"), damage, knockBack, player.whoAmI, current, 0f);
                Main.projectile[current].timeLeft = Main.projectile[head].timeLeft;
                Main.projectile[previous].localAI[1] = current;
                Main.projectile[previous].netUpdate = true;

                shootNum = 0;
            }
            //shoot body
            else
            {
                current = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("DestroyerBody"), damage, knockBack, player.whoAmI, current, 0f);
                Main.projectile[current].timeLeft = Main.projectile[head].timeLeft;

                previous = current;

                shootNum++;
            }



            

            //int previous = 0;

            /*for (int i = 0; i < 8; i++)
            {
                current = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("DestroyerBody"), damage, knockBack, player.whoAmI, current, 0f);
                previous = current;
            }

            current = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("DestroyerTail"), damage, knockBack, player.whoAmI, current, 0f);*/

            //Main.projectile[previous].localAI[1] = current;
            //Main.projectile[previous].netUpdate = true;

            return false;
        }
    }
}