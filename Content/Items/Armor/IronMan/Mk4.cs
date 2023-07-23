using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(4), HasFrames]
public class Mk4Helmet : IronManArmorHelmet<Mk4Chestplate, Mk4Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(4), HasFrames]
public class Mk4Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>(), ModContent.ItemType<ArsenalMicroGun>(), ModContent.ItemType<ArsenalMicroMissile>(), ModContent.ItemType<ArsenalFlares>() };
}

[AutoloadEquip(EquipType.Legs), Mark(4), HasFrames]
public class Mk4Leggings : IronManArmorLeggings { }