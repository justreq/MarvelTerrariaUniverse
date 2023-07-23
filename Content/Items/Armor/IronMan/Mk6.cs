using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(6), HasFrames]
public class Mk6Helmet : IronManArmorHelmet<Mk6Chestplate, Mk6Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(6), HasFrames]
public class Mk6Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>(), ModContent.ItemType<ArsenalMicroGun>(), ModContent.ItemType<ArsenalMicroMissile>(), ModContent.ItemType<ArsenalFlares>(), ModContent.ItemType<ArsenalLaserSystem>(), ModContent.ItemType<ArsenalPropelledGrenades>() };
}

[AutoloadEquip(EquipType.Legs), Mark(6), HasFrames]
public class Mk6Leggings : IronManArmorLeggings { }