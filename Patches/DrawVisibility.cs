using System;
using MarvelTerrariaUniverse.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;

namespace MarvelTerrariaUniverse.Patches;
internal sealed class DrawVisibility : BasePatch
{
    internal override void Patch(Mod mod)
    {
        MonoModHooks.Add(typeof(AccessorySlotLoader).GetMethod("DrawVisibility", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance), On_DrawVisibility);
        MonoModHooks.Modify(typeof(AccessorySlotLoader).GetMethod("DrawVisibility", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance), DrawVisibilityPatch);
    }

    static void DrawVisibilityPatch(ILContext il)
    {
        ILCursor c = new(il);
        ILLabel target = null;

        c.GotoNext(MoveType.After, i => i.MatchCallOrCallvirt(typeof(Rectangle).GetMethod("Contains", new Type[] { typeof(Point) })), i => i.MatchBrfalse(out target));

        c.EmitDelegate(() => Main.LocalPlayer.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout);
        c.Emit(OpCodes.Brtrue, target);
    }

    delegate bool orig_DrawVisibility(AccessorySlotLoader self, ref bool visbility, int context, int xLoc, int yLoc, out int xLoc2, out int yLoc2, out Texture2D value4);
    private bool On_DrawVisibility(orig_DrawVisibility orig, AccessorySlotLoader self, ref bool visbility, int context, int xLoc, int yLoc, out int xLoc2, out int yLoc2, out Texture2D value4)
    {
        bool skipCheck = orig(self, ref visbility, context, xLoc, yLoc, out xLoc2, out yLoc2, out value4);

        if (Main.LocalPlayer.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout)
        {
            value4 = Asset<Texture2D>.DefaultValue;
            skipCheck = false;
        }

        return skipCheck;
    }
}
