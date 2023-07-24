using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(7), HasFrames]
public class Mk7Helmet : IronManArmorHelmet<Mk7Chestplate, Mk7Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(7), HasFrames]
public class Mk7Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>(), ModContent.ItemType<ArsenalLaserSystem>(), ModContent.ItemType<ArsenalMicroMissile>(), ModContent.ItemType<ArsenalFlares>(), ModContent.ItemType<ArsenalSpreadGrenades>(), ModContent.ItemType<ArsenalSideWinders>() };
}

[AutoloadEquip(EquipType.Legs), Mark(7), HasFrames]
public class Mk7Leggings : IronManArmorLeggings { }