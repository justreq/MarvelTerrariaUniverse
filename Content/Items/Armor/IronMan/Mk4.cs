using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using MarvelTerrariaUniverse.Content.Buffs;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(4), HasFrames]
public class Mk4Helmet : IronManArmorHelmet<Mk4Chestplate, Mk4Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(4), HasFrames]
public class Mk4Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>(), ModContent.ItemType<ArsenalMicroGun>(), ModContent.ItemType<ArsenalMicroMissile>(), ModContent.ItemType<ArsenalFlares>() };
    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);
        if (player.wet)
        {
            player.AddBuff(ModContent.BuffType<Waterlogged>(), 150);
        }
        //same as mark 3
        player.moveSpeed *= 1.2f;
        player.jumpSpeedBoost *= 1.2f;
        player.statDefense += 48;
        player.buffImmune[BuffID.Frozen] = true;
        player.buffImmune[BuffID.Chilled] = true;
        player.buffImmune[BuffID.OnFire] = true;

        //slightly increase life regen
        player.lifeRegen += 2;
    }
}

[AutoloadEquip(EquipType.Legs), Mark(4), HasFrames]
public class Mk4Leggings : IronManArmorLeggings { }