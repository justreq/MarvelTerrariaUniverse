using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(1), HasFrames]
public class Mk1Helmet : IronManArmorHelmet<Mk1Chestplate, Mk1Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(1)]
public class Mk1Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalCrudeFlamethrower>(), ModContent.ItemType<ArsenalCrudeMissile>() };
}

[AutoloadEquip(EquipType.Legs), Mark(1)]
public class Mk1Leggings : IronManArmorLeggings { }