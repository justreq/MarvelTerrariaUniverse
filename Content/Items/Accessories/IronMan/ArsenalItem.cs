using MarvelTerrariaUniverse.Common.CustomLoadout;
using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.Content.Buffs;
using MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
public abstract class ArsenalItem : ModItem
{
    public bool Selected { get; set; }
    public int SlotIndex;

    private KeyboardState previousState;
    private KeyboardState currentState;

    public List<Keys> SlotKeys = new() { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0 };

    public bool JustPressed(Keys key)
    {
        return currentState.IsKeyDown(key) && previousState.IsKeyUp(key) && !Main.drawingPlayerChat;
    }

    public override void SetDefaults()
    {
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        IronManPlayer modPlayer = player.GetModPlayer<IronManPlayer>();

        if (modPlayer.Mark == 0) return;

        var arsenalSlots = modPlayer.ArsenalSlots;
        List<int> arsenal = ((IronManArmorChestplate)player.armor[1].ModItem).Arsenal;

        SlotIndex = arsenal.IndexOf(arsenal.Where(e => e == Item.type).First());

        previousState = currentState;
        currentState = Keyboard.GetState();

        if (JustPressed(SlotKeys[SlotIndex]))
        {
            SoundEngine.PlaySound(SoundID.MenuTick);

            if (Selected) Selected = false;
            else
            {
                for (int i = 0; i < arsenal.Count; i++)
                {
                    ((ArsenalItem)arsenalSlots[i].FunctionalItem.ModItem).Selected = false;
                }

                Selected = !Selected;
            }
        }

        ((BaseCustomLoadoutAccessorySlot)arsenalSlots[SlotIndex]).SlotBackgroundTexture = $"{nameof(MarvelTerrariaUniverse)}/Assets/UI/InventoryBack{(Selected ? "Selected" : "")}_IronMan";

        if (Selected && Main.mouseLeft) UpdateArsenal(player);
    }

    public virtual void UpdateArsenal(Player player) {
        if (player.HasBuff(ModContent.BuffType<Waterlogged>()))
        {
            return;
        }
        else if (player.HasBuff(BuffID.Frozen))
        {
            return;
        }
    }
}
