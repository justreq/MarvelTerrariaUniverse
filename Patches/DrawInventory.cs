using MarvelTerrariaUniverse.Common.Players;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Patches;
internal sealed class DrawInventory : BasePatch
{
    internal override void Patch(Mod mod)
    {
        IL_Main.DrawInventory += IL_Main_DrawInventory;
    }

    private void IL_Main_DrawInventory(MonoMod.Cil.ILContext il)
    {
        ILCursor c = new(il);

        c.GotoNext(i => i.MatchLdcI4(0), i => i.MatchStloc(104));
        c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(47));

        c.EmitDelegate((int orig) =>
        {
            return Main.LocalPlayer.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout ? 0 : orig;
        });
    }
}
