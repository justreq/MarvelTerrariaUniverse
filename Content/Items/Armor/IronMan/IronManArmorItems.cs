using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
[AttributeUsage(AttributeTargets.Class)]
public class Mark : Attribute
{
    public readonly int mark;

    public Mark(int mark)
    {
        this.mark = mark;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class HasFrames : Attribute { }

public abstract class IronManArmorHelmet<Chestplate, Leggings> : ModItem where Chestplate : ModItem where Leggings : ModItem
{
    private int Mark => ((Mark)GetType().GetCustomAttributes(typeof(Mark), false)[0]).mark;
    private bool HasFrames => GetType().IsDefined(typeof(HasFrames), false);

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server || !HasFrames) return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}Alt1", EquipType.Head, name: $"Mk{Mark}Helmet_HeadAlt1");
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}Alt2", EquipType.Head, name: $"Mk{Mark}Helmet_HeadAlt2");
    }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        if (Main.netMode == NetmodeID.Server || !HasFrames) return;

        int equipSlotHeadAlt1 = EquipLoader.GetEquipSlot(Mod, $"Mk{Mark}Helmet_HeadAlt1", EquipType.Head);
        int equipSlotHeadAlt2 = EquipLoader.GetEquipSlot(Mod, $"Mk{Mark}Helmet_HeadAlt2", EquipType.Head);

        ArmorIDs.Head.Sets.DrawHead[equipSlotHeadAlt1] = true;
        ArmorIDs.Head.Sets.DrawHead[equipSlotHeadAlt2] = true;
        if (Mark == 1) ArmorIDs.Head.Sets.DrawHatHair[equipSlotHeadAlt2] = true;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ModContent.ItemType<Chestplate>() && legs.type == ModContent.ItemType<Leggings>();
    }
}

public abstract class IronManArmorChestplate : ModItem
{
    private int Mark => ((Mark)GetType().GetCustomAttributes(typeof(Mark), false)[0]).mark;
    private bool HasFrames => GetType().IsDefined(typeof(HasFrames), false);

    public virtual List<int> Arsenal => new();

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server || !HasFrames) return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}Alt", EquipType.Body, name: $"Mk{Mark}Chestplate_BodyAlt");
    }

    public override void SetStaticDefaults()
    {
        if (Main.netMode == NetmodeID.Server || !HasFrames) return;

        int equipSlotBodyAlt = EquipLoader.GetEquipSlot(Mod, $"Mk{Mark}Chestplate_BodyAlt", EquipType.Body);

        ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBodyAlt] = true;
        ArmorIDs.Body.Sets.HidesArms[equipSlotBodyAlt] = true;
    }
}

public abstract class IronManArmorLeggings : ModItem
{
    private int Mark => ((Mark)GetType().GetCustomAttributes(typeof(Mark), false)[0]).mark;
    private bool HasFrames => GetType().IsDefined(typeof(HasFrames), false);

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server || !HasFrames) return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}Alt", EquipType.Legs, name: $"Mk{Mark}Leggings_LegsAlt");
    }

    public override void SetStaticDefaults()
    {
        if (Main.netMode == NetmodeID.Server || !HasFrames) return;

        int equipSlotLegsAlt = EquipLoader.GetEquipSlot(Mod, $"Mk{Mark}Leggings_LegsAlt", EquipType.Legs);

        ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegsAlt] = true;
    }
}