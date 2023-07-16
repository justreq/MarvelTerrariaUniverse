using Terraria.Audio;
using MarvelTerrariaUniverse.Common.PlayerLayers;
using MarvelTerrariaUniverse.Common.PlayerLayers.IronMan;
using MarvelTerrariaUniverse.Common.Systems;
using MarvelTerrariaUniverse.Content.Mounts;
using MarvelTerrariaUniverse.Content.Projectiles.IronMan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;
using MarvelTerrariaUniverse.Utilities.Extensions;
using System.Collections.Generic;
using Terraria.ID;
using ReLogic.Utilities;
using MarvelTerrariaUniverse.AssetManager;
using System.Linq;
using Terraria.ModLoader.IO;
using MarvelTerrariaUniverse.Common.UIElements.SuitModuleHubUI;
using MarvelTerrariaUniverse.Common.UIElements;
using MarvelTerrariaUniverse.Common.SuitModuleHubUI;
using MarvelTerrariaUniverse.Core.IronMan;

namespace MarvelTerrariaUniverse.Common.Players;
public class IronManPlayer : ModPlayer
{
    public bool CanSelectSuit = false;

    public Vector2 LastAccessedSuitModuleHubPosition = Vector2.Zero;

    public int RotationCooldown = 90;

    public bool IsTransformed => Player.GetModPlayer<BasePlayer>().transformation == MarvelTerrariaUniverse.Transformation.IronMan;

    public IronManSuit IronManSuit = null;

    public int? Mark = null;

    public int FaceplateFrame = 0;
    public int FaceplateFramerate = 5;
    public bool FaceplateOn = true;
    public bool FaceplateMoving = false;

    public bool Flying = false;
    public bool Hovering => Flying && !Player.controlUp && !Player.controlDown && !Player.controlLeft && !Player.controlRight;

    public float HeadRotation = 0f;

    public int FlightFlameFrame = 0;
    public int FlightFlameFramerate = 5;

    public bool HelmetDropped = false;
    public bool HelmetOn = true;

    public bool SuitEjected = false;
    public bool SuitOn = true;
    public int SuitDirection = 1;

    public void EquipSuit(int? mark = null)
    {
        Mark = mark;
        IronManSuit = mark == null ? null : IronManAbilities.IronManSuits[(int)mark - 1];
        Player.GetModPlayer<BasePlayer>().transformation = mark == null ? MarvelTerrariaUniverse.Transformation.None : MarvelTerrariaUniverse.Transformation.IronMan;

        if (mark == null) ResetEverything();
    }

    public void ResetEverything(bool soft = false)
    {
        if (!soft)
        {
            IronManSuit = null;
            Mark = null;
            SuitEjected = false;
            SuitOn = true;
            SuitDirection = 1;
        }

        RotationCooldown = 90;
        FaceplateFrame = 0;
        FaceplateFramerate = 5;
        FaceplateOn = true;
        FaceplateMoving = false;
        Flying = false;
        Player.mount.Dismount(Player);
        HeadRotation = 0f;
        FlightFlameFrame = 0;
        FlightFlameFramerate = 5;
        HelmetDropped = false;
        HelmetOn = true;
    }

    public void FlightAnimation()
    {
        var offset = Main.MouseWorld - Player.Center;
        var targetRot = 0f;

        if (!Flying) targetRot = 0f;
        else
        {
            FlightFlameFramerate++;

            if (FlightFlameFramerate > 5)
            {
                if (FlightFlameFrame >= (Hovering ? 1 : 2)) FlightFlameFrame = 0;
                else FlightFlameFrame++;

                FlightFlameFramerate = 0;
            }

            var distanceOffset = Player.Hitbox.Size() / 2f * 1.5f - new Vector2(Player.width - 2.5f, 0f);
            var center = Player.Hitbox.Location.ToVector2() + new Vector2(0f, 10f) + distanceOffset.RotatedBy(Player.fullRotation);

            var dust = Dust.NewDustDirect(center, Player.width, Player.height / 2, DustID.Smoke, 0f, 0f, 100, Scale: 0.5f);
            dust.scale *= 1f + Main.rand.Next(10) * 0.1f;
            dust.velocity *= 0.2f;
            dust.noGravity = true;

            if (Main.rand.NextBool(Hovering ? 5 : 1))
            {
                var dust2 = Dust.NewDustDirect(center, Player.width, Player.height / 2, DustID.Torch, Player.velocity.X * 0.2f, Player.velocity.Y * 0.2f, 100, Color.Yellow, 2f);
                dust2.noGravity = true;
                dust2.velocity *= 1.4f;
                dust2.velocity += Main.rand.NextVector2Circular(1f, 1f);
                dust2.velocity += Player.velocity * 0.15f;
            }

            Player.legFrame.Y = 0;

            if (Player.velocity.LengthSquared() > Math.Pow(7f, 2)) Player.velocity = Player.velocity.SafeNormalize(Vector2.Zero) * 7f;

            if (Hovering && RotationCooldown <= 0)
            {
                Player.direction = Math.Sign(offset.X);
                if (Math.Sign(offset.X) == Player.direction) targetRot = (offset * Player.direction).ToRotation() * 0.55f;
            }
            else targetRot = 0.8f * -Player.direction;
        }

        HeadRotation = HeadRotation.AngleLerp(targetRot, 15f * (1f / 60));
    }

    public void FaceplateAnimation()
    {
        if (!FaceplateMoving) return;

        FaceplateFramerate--;

        if (FaceplateFramerate <= 0)
        {
            FaceplateFrame = MathHelperExtensions.Step(FaceplateFrame, FaceplateOn ? 2 : 0, 1);
            FaceplateFramerate = 5;
        }

        if (FaceplateFrame == (FaceplateOn ? 2 : 0))
        {
            FaceplateMoving = false;
            FaceplateOn = !FaceplateOn;
        }
    }

    public void DropHelmet()
    {
        if (HelmetOn) return;

        if (!HelmetDropped)
        {
            Projectile.NewProjectile(Terraria.Entity.GetSource_None(), new Vector2(Player.Center.X + 20 * Player.direction, Player.Center.Y), new Vector2(Player.velocity.X, 0f), ModContent.ProjectileType<Helmet>(), 0, 0, Player.whoAmI);

            HelmetDropped = true;
        }
    }

    public void EjectSuit()
    {
        if (SuitOn) return;

        if (!SuitEjected)
        {
            SuitDirection = Player.direction;
            Projectile.NewProjectile(Terraria.Entity.GetSource_None(), Player.Center, Player.velocity, ModContent.ProjectileType<Suit>(), 0, 0, Player.whoAmI);

            SuitEjected = true;
            ResetEverything(true);
        }
    }

    public override void ResetEffects()
    {

    }

    public override void PostUpdate()
    {
        if (!IsTransformed) return;

        FlightAnimation();
        FaceplateAnimation();
        DropHelmet();
        EjectSuit();
    }

    public override void FrameEffects()
    {
        if (!IsTransformed) return;

        var headName = $"IronManMark{Mark}" + (FaceplateFrame == 1 ? "Alt" : FaceplateFrame == 2 ? "Alt2" : "");
        var bodyName = $"IronManMark{Mark}" + (Flying ? "Alt" : "");

        if (SuitOn && HelmetOn) Player.head = EquipLoader.GetEquipSlot(Mod, headName, EquipType.Head);
        if (SuitOn) Player.body = EquipLoader.GetEquipSlot(Mod, bodyName, EquipType.Body);
        if (SuitOn) Player.legs = EquipLoader.GetEquipSlot(Mod, $"IronManMark{Mark}" + (Hovering ? "Alt" : ""), EquipType.Legs);

        if (Main.dedServ) return;

        var path = $"{Assets.Textures.Path}/Glowmasks/IronMan";

        if (SuitOn && HelmetOn && FaceplateFrame != 2 && FaceplateOn && Mark != 1)
        {
            HelmetGlowmask.RegisterData(EquipLoader.GetEquipSlot(Mod, headName, EquipType.Head), new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>($"{path}/Faceplate{FaceplateFrame}"),
                Color = (drawInfo) => Color.White
            });
        }

        BasePlayer.RegisterData(EquipLoader.GetEquipSlot(Mod, bodyName, EquipType.Body), () => Color.White);
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.drawPlayer == UITransformationCharacter.preview)
        {
            drawInfo.colorArmorHead = Color.White;
            drawInfo.colorArmorBody = Color.White;
            drawInfo.colorArmorLegs = Color.White;
        }

        if (!IsTransformed) return;

        var drawPlayer = drawInfo.drawPlayer;
        drawInfo.rotationOrigin = drawPlayer.Hitbox.Size() / 2f;

        if (SuitOn) Lighting.AddLight((int)drawPlayer.position.X / 16, (int)drawPlayer.position.Y / 16, TorchID.Torch, 0.5f);

        if (Flying)
        {
            if (Hovering)
            {
                RotationCooldown--;

                if (RotationCooldown <= 0) drawPlayer.fullRotation = drawPlayer.fullRotation.AngleLerp(((Main.MouseWorld - drawPlayer.Center) * drawPlayer.direction).ToRotation() * 0.55f, 0.05f);
            }
            else
            {
                RotationCooldown = 90;
                drawPlayer.fullRotation = drawPlayer.fullRotation.AngleLerp(drawPlayer.velocity.ToRotation() + MathHelper.PiOver2, 0.075f);
            }
        }

        drawPlayer.headRotation = HeadRotation;
    }

    public override void SetControls()
    {
        if (!IsTransformed) return;

        if (Flying)
        {
            Player.controlJump = false;
            Player.controlMount = false;
        }
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (!IsTransformed || Mark == null || Mark == 1 || !SuitOn) return;

        if (KeybindSystem.ToggleFlight.JustPressed)
        {
            Flying = !Flying;

            if (Flying) Player.mount.SetMount(ModContent.MountType<IronManFlight>(), Player, Player.direction == -1);
            else Player.mount.Dismount(Player);
        }

        if (KeybindSystem.ToggleFaceplate.JustPressed && HelmetOn)
        {
            SoundEngine.PlaySound(Assets.ToSoundStyle(!FaceplateOn ? Assets.Sounds.IronMan.FaceplateOn : Assets.Sounds.IronMan.FaceplateOff));
            FaceplateMoving = true;
        }

        if (KeybindSystem.DropHelmet.JustPressed) HelmetOn = false;

        if (KeybindSystem.EjectSuit.JustPressed)
        {
            SoundEngine.PlaySound(Assets.ToSoundStyle(Assets.Sounds.IronMan.Depower));
            SuitOn = false;
        }
    }

    public override void SaveData(TagCompound tag)
    {
        tag.SaveNullable("mark", Mark);
        if (IronManSuit != null) tag["ironManSuit"] = IronManSuit;
    }

    public override void LoadData(TagCompound tag)
    {
        Mark = tag.LoadNullable<int>("mark");
        tag.TryGet("ironManSuit", ref IronManSuit);
    }
}