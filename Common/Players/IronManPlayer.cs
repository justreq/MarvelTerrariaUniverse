using MarvelTerrariaUniverse.Common.CustomLoadout;
using MarvelTerrariaUniverse.Common.Net;
using MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
using MarvelTerrariaUniverse.Content.Mounts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MarvelTerrariaUniverse.Common.Players;
public class IronManPlayer : ModPlayer
{
    public enum SuitState
    {
        None = 0,
        Flying = 1,
        Hovering = 2
    }

    public enum HelmetState
    {
        Closed = 0,
        HalfOpened = 1,
        Opened = 2,
    }

    public List<ModAccessorySlot> ArsenalSlots = ModContent.GetContent<ModAccessorySlot>().Where(x => x.Mod.Name == nameof(MarvelTerrariaUniverse) && x is BaseCustomLoadoutAccessorySlot).ToList();

    public SuitState CurrentSuitState = SuitState.None;
    public HelmetState CurrentHelmetState = HelmetState.Closed;

    public int Mark = 0;
    public int LastUsedLoadoutIndex = 0;

    public int SuitCycleTimer = 0;

    public bool HelmetClosed = true;
    public bool PlayFaceplateAnimation = false;
    public int FaceplateAnimationTimer = 0;

    public Color EyeSlitColor = Color.White;
    public int EyeSlitColorTransitionTimer = 0;

    public int FlightFlameFrameCounter = 0;
    public int FlightFlameAnimationTimer = 0;

    public void UnequipSuit()
    {
        Mark = 0;
        Player.GetModPlayer<LoadoutPlayer>().ClearCurrentLoadout();
        Player.TrySwitchingLoadout(LastUsedLoadoutIndex);
    }

    public void EquipSuit(int mark)
    {
        if (Mark == mark || mark < 1 || mark > MarvelTerrariaUniverse.IRONMANSUITS) return;

        ArsenalSlots.ForEach(e => e.FunctionalItem.TurnToAir());

        Mark = mark;
        LastUsedLoadoutIndex = Player.CurrentLoadoutIndex;
        List<int> arsenal = ((IronManArmorChestplate)Mod.Find<ModItem>($"Mk{Mark}Chestplate")).Arsenal;

        Player.GetModPlayer<LoadoutPlayer>().TrySwitchingVanillaToCustom(0);
        Player.armor[0] = new Item(Mod.Find<ModItem>($"Mk{Mark}Helmet").Type);
        Player.armor[1] = new Item(Mod.Find<ModItem>($"Mk{Mark}Chestplate").Type);
        Player.armor[2] = new Item(Mod.Find<ModItem>($"Mk{Mark}Leggings").Type);

        for (int i = 0; i < Player.hideVisibleAccessory.Length; i++)
        {
            Player.hideVisibleAccessory[i] = true;
        }

        for (int i = 0; i < arsenal.Count; i++)
        {
            ArsenalSlots[i].FunctionalItem = new Item(arsenal[i]);
        }
    }

    public void CycleSuits(int rate)
    {
        SuitCycleTimer++;

        if (SuitCycleTimer >= rate)
        {
            EquipSuit(Mark == MarvelTerrariaUniverse.IRONMANSUITS ? 1 : Mark + 1);
            CombatText.NewText(Player.Hitbox, Color.Red, $"Mark {Mark}");
            SuitCycleTimer = 0;
        }
    }

    public void UpdateFaceplateAnimation(int framerate)
    {
        FaceplateAnimationTimer++;

        if (FaceplateAnimationTimer > framerate)
        {
            CurrentHelmetState += HelmetClosed ? 1 : -1;
            FaceplateAnimationTimer = 0;
            new EquipSlotChangePacket(Player.whoAmI, Player.head, Player.body, Player.legs).Send();
        }

        if (CurrentHelmetState == (HelmetClosed ? HelmetState.Opened : HelmetState.Closed))
        {
            HelmetClosed = !HelmetClosed;
            PlayFaceplateAnimation = false;
        }
    }

    public void EyeSlitColorEasterEgg()
    {
        if (Mark == 1) return;

        if (Player.sitting.isSitting && Player.sitting.details.IsAToilet)
        {
            EyeSlitColorTransitionTimer++;

            if (EyeSlitColorTransitionTimer > 615) EyeSlitColor = Main.DiscoColor;
            switch (EyeSlitColorTransitionTimer)
            {
                case 300:
                    CombatText.NewText(Player.Hitbox, Color.White, "You know", true);
                    break;
                case 360:
                    CombatText.NewText(Player.Hitbox, Color.White, "The question I get asked most often is", true);
                    break;
                case 480:
                    CombatText.NewText(Player.Hitbox, Color.White, "\"how do you go to the bathroom in the suit\"", true);
                    break;
                case 750:
                    CombatText.NewText(Player.Hitbox, Color.White, "Just like that...", true);
                    break;
            }
        }
        else
        {
            EyeSlitColorTransitionTimer = 0;
            EyeSlitColor = Color.White;
        }
    }

    public void ToggleFlight()
    {
        CurrentSuitState = CurrentSuitState != SuitState.None ? SuitState.None : SuitState.Flying;

        if (CurrentSuitState != SuitState.None)
        {
            Player.mount.SetMount(ModContent.MountType<IronManFlight>(), Player, Player.direction == -1);
        }
        else
        {
            Player.mount.Dismount(Player);
        }
    }

    public void UpdateFlight()
    {
        if ((!Player.controlUp && !Player.controlRight && !Player.controlDown && !Player.controlLeft && !Player.controlJump) || ((Math.Abs(Player.velocity.X) < 0.1f && Math.Abs(Player.velocity.Y) < 0.1f))) CurrentSuitState = SuitState.Hovering;
        else CurrentSuitState = SuitState.Flying;

        Player.legFrame.Y = 0;
        Player.fullRotationOrigin = Player.Hitbox.Size() / 2;

        Vector2 mouseOffset = Main.MouseWorld - Player.Center;

        int targetDirection;
        float targetFullRotation;
        float targetFullRotationAmount;

        if (CurrentSuitState != SuitState.Hovering)
        {
            targetDirection = Math.Sign(mouseOffset.RotatedBy(-Player.fullRotation).X);
            targetFullRotation = Player.velocity.ToRotation() + MathHelper.PiOver2;
            targetFullRotationAmount = 0.075f;
        }
        else
        {
            targetDirection = Math.Sign(mouseOffset.X);
            targetFullRotation = (mouseOffset * Player.direction).ToRotation() * 0.55f;
            targetFullRotationAmount = 0.05f;
        }

        Player.direction = targetDirection;
        Player.fullRotation = Utils.AngleLerp(Player.fullRotation, targetFullRotation, targetFullRotationAmount);

        if (Mark == 1)
        {
            // Rocket boots dust bullshit lolxd pls help with this im lazy thx bye luv you bye
        }
        else
        {
            var distanceOffset = Player.Hitbox.Size() / 2f * 1.5f - new Vector2(Player.width - 2.5f, 0f);
            var center = Player.Hitbox.Location.ToVector2() + new Vector2(0f, 10f) + distanceOffset.RotatedBy(Player.fullRotation);

            var dust = Dust.NewDustDirect(center, Player.width, Player.height / 2, DustID.Smoke, 0f, 0f, 100, Scale: 0.5f);
            dust.scale *= 1f + Main.rand.Next(10) * 0.1f;
            dust.velocity *= 0.2f;
            dust.noGravity = true;

            if (Main.rand.NextBool(CurrentSuitState == SuitState.Hovering ? 5 : 1))
            {
                var dust2 = Dust.NewDustDirect(center, Player.width, Player.height / 2, DustID.Torch, Player.velocity.X * 0.2f, Player.velocity.Y * 0.2f, 100, Color.Yellow, 2f);
                dust2.noGravity = true;
                dust2.velocity *= 1.4f;
                dust2.velocity += Main.rand.NextVector2Circular(1f, 1f);
                dust2.velocity += Player.velocity * 0.15f;
            }
        }

        UpdateFlightFlameAnimation(5);
    }

    public void UpdateFlightFlameAnimation(int framerate)
    {
        FlightFlameAnimationTimer++;

        if (FlightFlameAnimationTimer >= framerate)
        {
            FlightFlameFrameCounter = FlightFlameFrameCounter == 6 ? 0 : FlightFlameFrameCounter + 1;
            FlightFlameAnimationTimer = 0;
        }
    }

    public override void PostUpdate()
    {
        if (Mark == 0) return;

        // CycleSuits(60);

        if (PlayFaceplateAnimation) UpdateFaceplateAnimation(5);

        EyeSlitColorEasterEgg();

        if (CurrentSuitState != SuitState.None) UpdateFlight();
    }

    public override void SetControls()
    {
        if (Mark == 0) return;

        PlayerInput.Triggers.JustPressed.Hotbar1 = false;
        PlayerInput.Triggers.JustPressed.Hotbar2 = false;
        PlayerInput.Triggers.JustPressed.Hotbar3 = false;
        PlayerInput.Triggers.JustPressed.Hotbar4 = false;
        PlayerInput.Triggers.JustPressed.Hotbar5 = false;
        PlayerInput.Triggers.JustPressed.Hotbar6 = false;
        PlayerInput.Triggers.JustPressed.Hotbar7 = false;
        PlayerInput.Triggers.JustPressed.Hotbar8 = false;
        PlayerInput.Triggers.JustPressed.Hotbar9 = false;
        PlayerInput.Triggers.JustPressed.Hotbar10 = false;
    }

    public override void FrameEffects()
    {
        if (Mark == 0) return;

        if (CurrentHelmetState != HelmetState.Closed) Player.head = EquipLoader.GetEquipSlot(Mod, $"Mk{Mark}Helmet_HeadAlt{(int)CurrentHelmetState}", EquipType.Head);

        if ((CurrentSuitState == SuitState.Flying || CurrentSuitState == SuitState.Hovering) && Mark != 1) Player.body = EquipLoader.GetEquipSlot(Mod, $"Mk{Mark}Chestplate_BodyAlt", EquipType.Body);

        if (CurrentSuitState == SuitState.Hovering && Mark != 1) Player.legs = EquipLoader.GetEquipSlot(Mod, $"Mk{Mark}Leggings_LegsAlt", EquipType.Legs);
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (Mark == 0) return;

        drawInfo.bodyGlowColor = Color.White;
        drawInfo.armGlowColor = Color.White;

        Vector2 mouseOffset = Main.MouseWorld - Player.Center;
        float targetHeadRotation;

        if (CurrentSuitState == SuitState.Flying) targetHeadRotation = 2f * -Player.direction;
        else if (CurrentSuitState == SuitState.Hovering && Math.Sign(mouseOffset.X) == Player.direction) targetHeadRotation = (mouseOffset * Player.direction).ToRotation() * 0.55f;
        else targetHeadRotation = 0f;

        Player.headRotation = Utils.AngleLerp(Player.headRotation, targetHeadRotation, 0.25f);
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (Mark == 0) return;

        if (KeybindSystem.ToggleFaceplate.JustPressed && !PlayFaceplateAnimation) PlayFaceplateAnimation = true;

        if (KeybindSystem.ToggleFlight.JustPressed) ToggleFlight();
    }

    public override void Load()
    {
        MTUNetMessages<EquipSlotChangePacket>.OnRecieve += RecieveEquipSlotChangePacket;
    }

    private static void RecieveEquipSlotChangePacket(in EquipSlotChangePacket data, int fromWho)
    {
        if (Main.netMode == NetmodeID.Server)
        {
            data.Send(-1, fromWho);
            Console.WriteLine("Resending plate message");
        }
        else
        {
            Player player = Main.player[data.Player];
            player.head = data.PlayerHead;
            player.body = data.PlayerBody;
            player.legs = data.PlayerLegs;
            Main.NewText($"Player: {data.Player} {player?.name ?? "null??"} is animating");
        }
    }

    public override void SaveData(TagCompound tag)
    {
        tag.Add(nameof(CurrentHelmetState), (int)CurrentHelmetState);
        tag.Add(nameof(Mark), Mark);
        tag.Add(nameof(LastUsedLoadoutIndex), LastUsedLoadoutIndex);
        tag.Add(nameof(SuitCycleTimer), SuitCycleTimer);
        tag.Add(nameof(HelmetClosed), HelmetClosed);
        tag.Add(nameof(PlayFaceplateAnimation), PlayFaceplateAnimation);
        tag.Add(nameof(FaceplateAnimationTimer), FaceplateAnimationTimer);
        tag.Add(nameof(EyeSlitColor), EyeSlitColor);
        tag.Add(nameof(EyeSlitColorTransitionTimer), EyeSlitColorTransitionTimer);
    }

    public override void LoadData(TagCompound tag)
    {
        if (tag.TryGet(nameof(CurrentHelmetState), out int currentHelmetState)) CurrentHelmetState = (HelmetState)currentHelmetState;
        if (tag.TryGet(nameof(Mark), out int mark)) Mark = mark;
        if (tag.TryGet(nameof(LastUsedLoadoutIndex), out int lastUsedLoadoutIndex)) LastUsedLoadoutIndex = lastUsedLoadoutIndex;
        if (tag.TryGet(nameof(SuitCycleTimer), out int suitCycleTimer)) SuitCycleTimer = suitCycleTimer;
        if (tag.TryGet(nameof(HelmetClosed), out bool helmetClosed)) HelmetClosed = helmetClosed;
        if (tag.TryGet(nameof(PlayFaceplateAnimation), out bool playFaceplateAnimation)) PlayFaceplateAnimation = playFaceplateAnimation;
        if (tag.TryGet(nameof(FaceplateAnimationTimer), out int faceplateAnimationTimer)) FaceplateAnimationTimer = faceplateAnimationTimer;
        if (tag.TryGet(nameof(EyeSlitColor), out Color eyeSlitColor)) EyeSlitColor = eyeSlitColor;
        if (tag.TryGet(nameof(EyeSlitColorTransitionTimer), out int eyeSlitColorTransitionTimer)) EyeSlitColorTransitionTimer = eyeSlitColorTransitionTimer;
    }
}
