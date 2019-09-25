﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework;
using CalamityMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class AtaxiaEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ataxia Enchantment");
            Tooltip.SetDefault(
@"'Not be confused with Ataraxia Enchantment'
You have a 20% chance to emit a blazing explosion on hit
Melee attacks and projectiles cause chaos flames to erupt on enemy hits
You have a 50% chance to fire a homing chaos flare when using ranged weapons
Magic attacks summon damaging and healing flare orbs on hit
Summons a chaos spirit to protect you
Rogue weapons have a 10% chance to unleash a volley of chaos flames around the player
Effects of the Plague Hive
Summons a Brimling pet");
            DisplayName.AddTranslation(GameCulture.Chinese, "阿塔西亚魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'别和禅心搞混了'
攻击有20%概率释放炎爆
近战攻击和抛射物会对敌人造成混乱之火
使用远程武器时有50%概率发射追踪的混乱之火
魔法攻击召唤治愈和伤害的火球
召唤混乱之灵保护你
盗贼武器有10%概率在玩家周围释放一串混乱之火
拥有瘟疫蜂巢的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 1000000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            CalamityPlayer modPlayer = player.GetModPlayer<CalamityPlayer>(calamity);

            if (SoulConfig.Instance.GetValue("Ataxia Effects"))
            {
                //all
                modPlayer.ataxiaBlaze = true;
                //melee
                modPlayer.ataxiaGeyser = true;
                //range
                modPlayer.ataxiaBolt = true;
                //magic
                modPlayer.ataxiaMage = true;
                //throw
                modPlayer.ataxiaVolley = true;
            }
            
            if (SoulConfig.Instance.GetValue("Plague Hive"))
            {
                //plague hive
                player.buffImmune[calamity.BuffType("Plague")] = true;
                modPlayer.uberBees = true;
                player.strongBees = true;
                modPlayer.alchFlask = true;
                int num = 0;
                Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0.1f, 2f, 0.2f);
                int num2 = calamity.BuffType("Plague");
                float num3 = 300f;
                bool flag = num % 60 == 0;
                int num4 = 60;
                int num5 = Main.rand.Next(10);
                if (player.whoAmI == Main.myPlayer && num5 == 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && !npc.friendly && npc.damage > 0 && !npc.dontTakeDamage && !npc.buffImmune[num2] && Vector2.Distance(player.Center, npc.Center) <= num3)
                        {
                            if (npc.FindBuffIndex(num2) == -1)
                            {
                                npc.AddBuff(num2, 120, false);
                            }
                            if (flag)
                            {
                                npc.StrikeNPC(num4, 0f, 0, false, false, false);
                                if (Main.netMode != 0)
                                {
                                    NetMessage.SendData(28, -1, -1, null, i, (float)num4, 0f, 0f, 0, 0, 0);
                                }
                            }
                        }
                    }
                }
                num++;
            }
            
            if (player.GetModPlayer<FargoPlayer>().Eternity) return;

            if (SoulConfig.Instance.GetValue("Chaos Spirit Minion"))
            {
                //summon
                modPlayer.chaosSpirit = true;
                if (player.whoAmI == Main.myPlayer)
                {
                    if (player.FindBuffIndex(calamity.BuffType("ChaosSpirit")) == -1)
                    {
                        player.AddBuff(calamity.BuffType("ChaosSpirit"), 3600, true);
                    }
                    if (player.ownedProjectileCounts[calamity.ProjectileType("ChaosSpirit")] < 1)
                    {
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, calamity.ProjectileType("ChaosSpirit"), (int)(190f * player.minionDamage), 0f, Main.myPlayer, 0f, 0f);
                    }
                }
            }

            FargoPlayer fargoPlayer = player.GetModPlayer<FargoPlayer>(mod);
            fargoPlayer.AtaxiaEnchant = true;
            fargoPlayer.AddPet("Brimling Pet", hideVisual, calamity.BuffType("BrimlingBuff"), calamity.ProjectileType("Brimling"));
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddRecipeGroup("FargowiltasSouls:AnyAtaxiaHelmet");
            recipe.AddIngredient(calamity.ItemType("AtaxiaArmor"));
            recipe.AddIngredient(calamity.ItemType("AtaxiaSubligar"));
            recipe.AddIngredient(calamity.ItemType("PlagueHive"));
            recipe.AddIngredient(calamity.ItemType("SpearofDestiny"));
            recipe.AddIngredient(calamity.ItemType("Hellborn"));
            recipe.AddIngredient(calamity.ItemType("Lucrecia"));
            recipe.AddIngredient(calamity.ItemType("BarracudaGun"));
            recipe.AddIngredient(calamity.ItemType("Vesuvius"));
            recipe.AddIngredient(calamity.ItemType("LeadWizard"));
            recipe.AddIngredient(calamity.ItemType("Malachite"));
            recipe.AddIngredient(calamity.ItemType("Impaler"));
            recipe.AddIngredient(calamity.ItemType("HolidayHalberd"));
            recipe.AddIngredient(calamity.ItemType("CharredRelic"));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
