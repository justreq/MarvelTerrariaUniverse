using MonoMod.Cil;
using System;
using Terraria.ModLoader;
using Terraria;
using Mono.Cecil.Cil;
using MarvelTerrariaUniverse.Common.Players;

namespace MarvelTerrariaUniverse.Patches;
internal sealed class TrySwitchingLoadout : BasePatch
{
    internal override void Patch(Mod mod)
    {
        IL_Player.TrySwitchingLoadout += IL_Player_TrySwitchingLoadout;
    }

    private void IL_Player_TrySwitchingLoadout(ILContext il)
    {
        ILCursor c = new(il);

        if (!c.TryGotoNext(MoveType.After, opcode => opcode.MatchLdarg(1)))
        {
            throw new Exception("Failed while patching TrySwitchingLoadout: could not match ldarg.1");
        }

        c.Emit(OpCodes.Ldarg_0);
        c.EmitDelegate<Func<int, Player, int>>((loadoutIndex, player) =>
        {
            if (player.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout) return 22;

            return loadoutIndex;
        });

        if (!c.TryGotoNext(MoveType.Before, opcode => opcode.MatchCallvirt<EquipmentLoadout>(nameof(EquipmentLoadout.Swap))))
        {
            throw new Exception("Failed while patching TrySwitchingLoadout: could not match callvirt");
        }

        c.Remove();

        c.EmitDelegate<Action<EquipmentLoadout, Player>>((loadout, player) =>
        {
            LoadoutPlayer modPlayer = player.GetModPlayer<LoadoutPlayer>();

            if (modPlayer.UsingCustomLoadout) modPlayer.ClearCustomForVanilla();
            else loadout.Swap(player);
        });
    }
}
