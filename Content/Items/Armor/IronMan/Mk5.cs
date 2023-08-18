using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using MarvelTerrariaUniverse.Content.Buffs;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(5), HasFrames]
public class Mk5Helmet : IronManArmorHelmet<Mk5Chestplate, Mk5Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(5), HasFrames]
public class Mk5Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>() };
    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);
        if (player.wet)
        {
            player.AddBuff(ModContent.BuffType<Waterlogged>(), 150);
        }
        // 15% increased movement speed
        player.moveSpeed *= 1.15f;
        // 24 defense
        player.statDefense += 24;
        // immune to on fire, chilled, and frozen
        player.buffImmune[BuffID.OnFire] = true;
    }
}

[AutoloadEquip(EquipType.Legs), Mark(5), HasFrames]
public class Mk5Leggings : IronManArmorLeggings { }