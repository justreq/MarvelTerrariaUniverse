using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using MarvelTerrariaUniverse.Common.CustomLoadout;

namespace MarvelTerrariaUniverse.Common.Players;
public class LoadoutPlayer : ModPlayer
{
    public EquipmentLoadout CurrentCustomLoadout => CustomLoadouts[CurrentCustomLoadoutIndex];

    public readonly EquipmentLoadout[] CustomLoadouts = new EquipmentLoadout[1] {
            new(),
        };

    public int CurrentCustomLoadoutIndex { get; set; }
    public bool UsingCustomLoadout => CurrentCustomLoadoutIndex != -1;

    public override void Initialize()
    {
        CurrentCustomLoadoutIndex = -1;
    }

    public override void SaveData(TagCompound tag)
    {
        if (UsingCustomLoadout)
        {
            tag.Add(nameof(CurrentCustomLoadoutIndex), CurrentCustomLoadoutIndex);
        }

        for (int i = 0; i < CustomLoadouts.Length; i++)
        {
            tag.Add(nameof(CustomLoadouts) + i, CustomLoadouts[i]);
        }
    }

    public override void LoadData(TagCompound tag)
    {
        if (tag.TryGet(nameof(CurrentCustomLoadoutIndex), out int currentCustomLoadoutIndex))
        {
            CurrentCustomLoadoutIndex = currentCustomLoadoutIndex;
        }

        for (int i = 0; i < CustomLoadouts.Length; i++)
        {
            if (tag.TryGet(nameof(CustomLoadouts) + i, out EquipmentLoadout loadout))
            {
                CustomLoadouts[i] = loadout;
            }
        }
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        for (int i = 0; i < MarvelTerrariaUniverse.EXTRALOADOUTS; i++)
        {
            LoadoutSyncing.SyncCustomLoadout(Player, i, toWho, fromWho);
        }
    }

    internal void TrySwitchingVanillaToCustom(int customLoadoutIndex)
    {
        if (IsPlayerReadyToSwitchLoadouts() && IsCustomLoadoutIndexValid(customLoadoutIndex))
        {
            Player.Loadouts[Player.CurrentLoadoutIndex].Swap(Player);
            CustomLoadouts[customLoadoutIndex].Swap(Player);
            CurrentCustomLoadoutIndex = customLoadoutIndex;

            if (Player.whoAmI == Main.myPlayer)
            {
                Main.mouseLeftRelease = false;

                // I'm not sure _what_ vanilla's CloneLoadOuts(Player) does and
                // this (see code below) crashes because Main.clientPlayer doesn't have ModPlayers
                //CloneExLoadouts(Main.clientPlayer);
                //CloneLoadouts(Main.clientPlayer);

                NetMessage.TrySendData(MessageID.SyncLoadout, -1, -1, null, Player.whoAmI, Player.CurrentLoadoutIndex);
                LoadoutSyncing.SyncCustomLoadout(Player, customLoadoutIndex, -1, Player.whoAmI);

                SoundEngine.PlaySound(SoundID.MenuTick);
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.LoadoutChange, new ParticleOrchestraSettings
                {
                    PositionInWorld = Player.Center,
                    UniqueInfoPiece = 0
                }, Player.whoAmI);

                ItemSlot.RecordLoadoutChange();
            }
        }
    }

    internal void TrySwitchingCustomToCustom(int customLoadoutIndex)
    {
        if (IsPlayerReadyToSwitchLoadouts() && IsCustomLoadoutIndexValid(customLoadoutIndex))
        {
            CustomLoadouts[CurrentCustomLoadoutIndex].Swap(Player);
            CustomLoadouts[customLoadoutIndex].Swap(Player);
            CurrentCustomLoadoutIndex = customLoadoutIndex;

            if (Player.whoAmI == Main.myPlayer)
            {
                Main.mouseLeftRelease = false;

                // I'm not sure _what_ vanilla's CloneLayouts(Player) does and
                // this (see code below) crashes because Main.clientPlayer doesn't have ModPlayers
                //CloneExLoadouts(Main.clientPlayer);
                LoadoutSyncing.SyncCustomLoadout(Player, customLoadoutIndex, -1, Player.whoAmI);

                SoundEngine.PlaySound(SoundID.MenuTick);
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.LoadoutChange, new ParticleOrchestraSettings
                {
                    PositionInWorld = Player.Center,
                    UniqueInfoPiece = 0
                }, Player.whoAmI);

                ItemSlot.RecordLoadoutChange();
            }
        }
    }

    private bool IsPlayerReadyToSwitchLoadouts()
    {
        return (Player.whoAmI != Main.myPlayer || (!IsUsingItem() && !Player.CCed && !Player.dead));
    }

    private bool IsCustomLoadoutIndexValid(int customLoadoutIndex)
    {
        return customLoadoutIndex != CurrentCustomLoadoutIndex && customLoadoutIndex >= 0 && customLoadoutIndex < CustomLoadouts.Length;
    }

    private bool IsUsingItem()
    {
        return Player.itemTime > 0 || Player.itemAnimation > 0;
    }

    internal void ClearCustomForVanilla()
    {
        CurrentCustomLoadout.Swap(Player);
        CurrentCustomLoadoutIndex = -1;
    }

    public void ClearCurrentLoadout()
    {
        for (int i = 0; i < Player.armor.Length; i++)
        {
            Player.armor[i].TurnToAir();
        }
    }
}
