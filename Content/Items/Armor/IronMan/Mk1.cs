using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
using System.Collections.Generic;
using System.Security.Policy;
using Terraria;
using MarvelTerrariaUniverse.Content.Buffs;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AutoloadEquip(EquipType.Head), Mark(1), HasFrames]
public class Mk1Helmet : IronManArmorHelmet<Mk1Chestplate, Mk1Leggings> { }

[AutoloadEquip(EquipType.Body), Mark(1)]
public class Mk1Chestplate : IronManArmorChestplate
{
    public override List<int> Arsenal => new() { ModContent.ItemType<ArsenalCrudeFlamethrower>(), ModContent.ItemType<ArsenalCrudeMissile>() };

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);
        if (player.wet)
        {
            player.AddBuff(ModContent.BuffType<Waterlogged>(), 150);
        }
        // reduce player speed by 50%
        player.moveSpeed *= 0.5f;
        // decrease player jump height by 25%
        player.jumpSpeedBoost *= 0.75f;
        //increase defense by 20
        player.statDefense += 20;
    }
}

[AutoloadEquip(EquipType.Legs), Mark(1)]
public class Mk1Leggings : IronManArmorLeggings { }