using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using MarvelTerrariaUniverse.Content.Buffs;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(3), HasFrames]
public class Mk3Helmet : IronManArmorHelmet<Mk3Chestplate, Mk3Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(3), HasFrames]
public class Mk3Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalRepulsor>(), ModContent.ItemType<ArsenalUnibeam>(), ModContent.ItemType<ArsenalMicroGun>(), ModContent.ItemType<ArsenalMicroMissile>(), ModContent.ItemType<ArsenalFlares>() };
    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);
        if (player.wet)
        {
            player.AddBuff(ModContent.BuffType<Waterlogged>(), 150);
        }
        // increase player speed by 20%
        player.moveSpeed *= 1.2f;
        // increase player jump height by 20%
        player.jumpSpeedBoost *= 1.2f;
        //increase defense by 48
        player.statDefense += 48;
        //grant buff immunity to frozen, chilled, and on fire
        player.buffImmune[BuffID.Frozen] = true;
        player.buffImmune[BuffID.Chilled] = true;
        player.buffImmune[BuffID.OnFire] = true;
    }
}

[AutoloadEquip(EquipType.Legs), Mark(3), HasFrames]
public class Mk3Leggings : IronManArmorLeggings { }