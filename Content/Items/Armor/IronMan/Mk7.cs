using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(7), HasFrames]
public class Mk7Helmet : IronManArmorHelmet<Mk7Chestplate, Mk7Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(7), HasFrames]
public class Mk7Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>(), ModContent.ItemType<ArsenalLaserSystem>(), ModContent.ItemType<ArsenalMicroMissile>(), ModContent.ItemType<ArsenalFlares>(), ModContent.ItemType<ArsenalSpreadGrenades>(), ModContent.ItemType<ArsenalSideWinders>() };
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

[AutoloadEquip(EquipType.Legs), Mark(7), HasFrames]
public class Mk7Leggings : IronManArmorLeggings { }