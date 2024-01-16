using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using MarvelTerrariaUniverse.Content.Buffs;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(2), HasFrames]
public class Mk2Helmet : IronManArmorHelmet<Mk2Chestplate, Mk2Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(2), HasFrames]
public class Mk2Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>() };
    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);
        //check if the player is in space biome
        if (player.ZoneSkyHeight)
        {
            //give frozen debuff for 5 seconds
            player.AddBuff(BuffID.Frozen, 150);
        }
        if (player.wet)
        {
            player.AddBuff(ModContent.BuffType<Waterlogged>(), 150);
        }
        player.moveSpeed *= 1.2f;
        player.jumpSpeedBoost *= 1.2f;
        //increase defense by 36
        player.statDefense += 36;

    }
}

[AutoloadEquip(EquipType.Legs), Mark(2), HasFrames]
public class Mk2Leggings : IronManArmorLeggings { }