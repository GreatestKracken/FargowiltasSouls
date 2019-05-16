﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Accessories.Masomode
{
    public class BionomicCluster : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bionomic Cluster");
            Tooltip.SetDefault(@"''
Grants immunity to Frostburn, Shadowflame, Frozen, Squeaky Toy, Purified, and Mighty Wind
Grants immunity to Flames of the Universe, Clipped Wings, Crippled, Webbed, and Suffocation
Your attacks have a 10% chance to inflict Clipped Wings on non-boss enemies
Your attacks summon Shadowfrostfireballs to attack your enemies
You respawn twice as fast when no boss is alive
Increases max life by 50 and flight time by 25%
Attacks have a chance to squeak and deal 1 damage to you
Summons a friendly rainbow slime");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 7;
            item.value = Item.sellPrice(0, 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer fargoPlayer = player.GetModPlayer<FargoPlayer>();

            //concentrated rainbow matter
            player.buffImmune[mod.BuffType("FlamesoftheUniverse")] = true;
            if (Soulcheck.GetValue("Rainbow Slime Minion"))
                player.AddBuff(mod.BuffType("RainbowSlime"), 2);

            //dragon fang
            player.buffImmune[mod.BuffType("ClippedWings")] = true;
            player.buffImmune[mod.BuffType("Crippled")] = true;
            if (Soulcheck.GetValue("Inflict Clipped Wings"))
                fargoPlayer.DragonFang = true;

            //frigid gemstone
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.ShadowFlame] = true;
            if (Soulcheck.GetValue("Shadowfrostfireballs"))
            {
                fargoPlayer.FrigidGemstone = true;
                if (fargoPlayer.FrigidGemstoneCD > 0)
                    fargoPlayer.FrigidGemstoneCD--;
            }

            //sands of time
            player.buffImmune[BuffID.WindPushed] = true;
            fargoPlayer.SandsofTime = true;

            //squeaky toy
            player.buffImmune[mod.BuffType("SqueakyToy")] = true;
            player.buffImmune[mod.BuffType("Purified")] = true;
            fargoPlayer.SqueakyAcc = true;

            //tribal charm
            player.buffImmune[BuffID.Webbed] = true;
            player.buffImmune[BuffID.Suffocation] = true;
            player.statLifeMax2 += 50;
            fargoPlayer.wingTimeModifier += 0.25f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("ConcentratedRainbowMatter"));
            recipe.AddIngredient(mod.ItemType("DragonFang"));
            recipe.AddIngredient(mod.ItemType("FrigidGemstone"));
            recipe.AddIngredient(mod.ItemType("SandofTime"));
            recipe.AddIngredient(mod.ItemType("SqueakyToy"));
            recipe.AddIngredient(mod.ItemType("TribalCharm"));

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
