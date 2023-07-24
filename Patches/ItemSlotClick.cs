using MarvelTerrariaUniverse.Common.Players;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using IContext = Terraria.UI.ItemSlot.Context;

namespace MarvelTerrariaUniverse.Patches;
internal sealed class ItemSlotClick : BasePatch
{
    static ReadOnlySpan<int> InvalidContexts => new int[] {
        IContext.EquipAccessory,
        IContext.EquipAccessoryVanity,
        IContext.ModdedAccessorySlot,
        IContext.ModdedVanityAccessorySlot,
        IContext.EquipArmor,
        IContext.EquipArmorVanity
    };

    static bool IsArmorOrAccessory(Item item) => item.accessory || item.bodySlot > 0 || item.headSlot > 0 || item.legSlot > 0;

    internal override void Patch(Mod mod)
    {
        On_ItemSlot.LeftClick_ItemArray_int_int += On_ItemSlot_LeftClick_ItemArray_int_int;
        On_ItemSlot.RightClick_ItemArray_int_int += On_ItemSlot_RightClick_ItemArray_int_int;
    }

    private void On_ItemSlot_LeftClick_ItemArray_int_int(On_ItemSlot.orig_LeftClick_ItemArray_int_int orig, Item[] inv, int context, int slot)
    {
        if (!Main.LocalPlayer.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout || !InvalidContexts.Contains(context))
            orig(inv, context, slot);
    }

    private void On_ItemSlot_RightClick_ItemArray_int_int(On_ItemSlot.orig_RightClick_ItemArray_int_int orig, Item[] inv, int context, int slot)
    {
        if (!Main.LocalPlayer.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout || (!InvalidContexts.Contains(context) && !IsArmorOrAccessory(inv[slot])))
            orig(inv, context, slot);
    }
}