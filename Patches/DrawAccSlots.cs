using MarvelTerrariaUniverse.Common.Players;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Patches;
internal sealed class DrawAccSlots : BasePatch
{
    internal override void Patch(Mod mod)
    {
        MonoModHooks.Modify(typeof(AccessorySlotLoader).GetMethod("DrawAccSlots"), DrawAccSlotsPatch);
    }

    static void DrawAccSlotsPatch(ILContext il)
    {
        ILCursor c = new(il);

        c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(0));
        c.Emit(OpCodes.Pop);
        c.EmitDelegate(() =>
        {
            if (!Main.LocalPlayer.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout) return 0;

            return 6 + Main.LocalPlayer.extraAccessorySlots;
        });

        c.TryGotoNext(i => i.MatchLdcI4(3), i => i.MatchStloc(5), i => i.MatchBr(out _));
        c.EmitDelegate(() =>
        {
            return Main.LocalPlayer.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout;
        });

        var label = c.DefineLabel();
        c.Emit(OpCodes.Brtrue, label);

        c.TryGotoNext(MoveType.After, i => i.MatchBlt(out _));
        c.MarkLabel(label);

        c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(64));
        c.Emit(OpCodes.Pop);
        c.EmitDelegate(() =>
        {
            return -28;
        });
    }
}