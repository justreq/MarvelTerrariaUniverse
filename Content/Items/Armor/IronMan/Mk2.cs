using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(2), HasFrames]
public class Mk2Helmet : IronManArmorHelmet<Mk2Chestplate, Mk2Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(2), HasFrames]
public class Mk2Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>() };
}

[AutoloadEquip(EquipType.Legs), Mark(2), HasFrames]
public class Mk2Leggings : IronManArmorLeggings { }