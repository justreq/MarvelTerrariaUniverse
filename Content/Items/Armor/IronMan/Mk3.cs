using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(3), HasFrames]
public class Mk3Helmet : IronManArmorHelmet<Mk3Chestplate, Mk3Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(3), HasFrames]
public class Mk3Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>(), ModContent.ItemType<ArsenalMicroGun>(), ModContent.ItemType<ArsenalMicroMissile>(), ModContent.ItemType<ArsenalFlares>() };
}

[AutoloadEquip(EquipType.Legs), Mark(3), HasFrames]
public class Mk3Leggings : IronManArmorLeggings { }