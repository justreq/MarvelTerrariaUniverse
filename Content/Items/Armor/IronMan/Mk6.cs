using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(6), HasFrames]
public class Mk6Helmet : IronManArmorHelmet<Mk6Chestplate, Mk6Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(6), HasFrames]
public class Mk6Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>(), ModContent.ItemType<ArsenalMicroGun>(), ModContent.ItemType<ArsenalMicroMissile>(), ModContent.ItemType<ArsenalFlares>(), ModContent.ItemType<ArsenalLaserSystem>(), ModContent.ItemType<ArsenalPropelledGrenades>() };
    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);
        // 20% increased movement speed
        player.moveSpeed *= 1.2f;
        // 50 defense
        player.statDefense += 50;
        // immune to on fire, chilled, frozen, and electrified
        player.buffImmune[BuffID.OnFire] = true;
        player.buffImmune[BuffID.Chilled] = true;
        player.buffImmune[BuffID.Frozen] = true;
        player.buffImmune[BuffID.Electrified] = true;

    }
}

[AutoloadEquip(EquipType.Legs), Mark(6), HasFrames]
public class Mk6Leggings : IronManArmorLeggings { }