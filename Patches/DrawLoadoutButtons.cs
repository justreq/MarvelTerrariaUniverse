using MarvelTerrariaUniverse.Common.Players;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Patches;
internal sealed class DrawLoadoutButtons : BasePatch
{
    internal override void Patch(Mod mod)
    {
        IL_Main.DrawLoadoutButtons += IL_Main_DrawLoadoutButtons;
    }

    private void IL_Main_DrawLoadoutButtons(MonoMod.Cil.ILContext il)
    {
        ILCursor c = new(il);
        ILLabel label = c.DefineLabel();

        c.Goto(0);
        c.EmitDelegate(() =>
        {
            return !Main.LocalPlayer.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout;
        });

        c.Emit(OpCodes.Brtrue, label);
        c.Emit(OpCodes.Ret);
        c.Emit(OpCodes.Nop, label);
        c.MarkLabel(label);
    }
}