using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(5), HasFrames]
public class Mk5Helmet : IronManArmorHelmet<Mk5Chestplate, Mk5Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(5), HasFrames]
public class Mk5Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>() };
}

[AutoloadEquip(EquipType.Legs), Mark(5), HasFrames]
public class Mk5Leggings : IronManArmorLeggings { }