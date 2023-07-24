using MonoMod.Cil;
using System;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;

namespace MarvelTerrariaUniverse.Patches;
internal sealed class AccCheckPatch : BasePatch
{
    internal override void Patch(Mod mod)
    {
        IL_ItemSlot.AccCheck_ForLocalPlayer += IL_ItemSlot_AccCheck_ForLocalPlayer;
    }

    private void IL_ItemSlot_AccCheck_ForLocalPlayer(ILContext il)
    {
        ILCursor c = new(il);

        if (!c.TryGotoNext(MoveType.Before, opcode => opcode.MatchCallvirt<Item>("IsTheSameAs")))
        {
            throw new Exception("Failed while patching AccCheck_ForLocalPlayer: couldn't match callvirt #1");
        }

        c.Remove();
        c.EmitDelegate<Func<Item, Item, bool>>((item0, item1) =>
        {
            return item0.netID == item1.netID && item0.type == item1.type;
        });

        if (!c.TryGotoNext(MoveType.Before, opcode => opcode.MatchCallvirt<Item>("IsTheSameAs")))
        {
            throw new Exception("Failed while patching AccCheck_ForLocalPlayer: couldn't match callvirt #2");
        }

        c.Remove();
        c.EmitDelegate<Func<Item, Item, bool>>((item0, item1) =>
        {
            return item0.netID == item1.netID && item0.type == item1.type;
        });
    }
}