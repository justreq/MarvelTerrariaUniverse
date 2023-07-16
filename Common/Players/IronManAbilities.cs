using MarvelTerrariaUniverse.AssetManager;
using MarvelTerrariaUniverse.Content.Projectiles.IronMan;
using MarvelTerrariaUniverse.Core.IronMan;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Common.Players;
public class IronManAbilities : ModPlayer
{
    public enum Weapon
    {
        Repulsor = 0,
        Unibeam = 1,
        ShoulderGun = 2,
        ForearmMissile = 3,
        Flare = 4
    }

    public List<Weapon> RequestedWeapons = new();

    public IronManPlayer IronManPlayer => Player.GetModPlayer<IronManPlayer>();
    public IronManSuit IronManSuit => IronManPlayer.IronManSuit;

    public bool AbilityUsable<T>() where T : Ability => IronManSuit.abilities.Any(e => e is T);
    public T TryGetAbility<T>() where T : Ability => IronManSuit.abilities.TryGetValue(typeof(T), out Ability ability) ? (T)ability : null;
    public bool AbilityRequested(Weapon weapon) => RequestedWeapons.Contains(weapon);

    public int RepulsorCooldown = 60;
    public SlotId RepulsorChargeSoundSlot;
    public int UnibeamCooldown = 180;
    public SlotId UnibeamChargeSoundSlot;

    public static List<IronManSuit> IronManSuits = new()
    {
        new(1, 10f, 10f, 10f, new()),
        new(2, 10f, 10f, 10f, new()),
        new(3, 10f, 10f, 10f, new() { { typeof(Repulsor), new Repulsor(Main.LocalPlayer.Center, Vector2.Zero, 0, 0, 60) } }),
        new(4, 10f, 10f, 10f, new()),
        new(5, 10f, 10f, 10f, new()),
        new(6, 10f, 10f, 10f, new()),
        new(7, 10f, 10f, 10f, new())
    };

    public List<NPC> GetNearestXNPCs(float maxDistance, int count)
    {
        return Main.npc.SkipLast(1).Where(e => e.active && e.WithinRange(Player.Center, maxDistance)).OrderBy(e => e.DistanceSQ(Player.Center)).Take(count).ToList();
    }

    List<NPC> npcs = new();
    List<NPC> targets = new();
    int timer = 15;
    int timer2 = 60;

    public void ShoulderGun()
    {
        if (!AbilityRequested(Weapon.ShoulderGun)) return;

        if (npcs.Count == 0 && timer2 == 60)
        {
            npcs = GetNearestXNPCs(750, 12);

            if (npcs.Count == 0)
            {
                RequestedWeapons.Remove(Weapon.ShoulderGun);
                return;
            }
        }

        timer--;

        if (timer == 0 && npcs.Count > 0)
        {
            timer = 15;

            targets.Add(npcs.First());
            npcs.RemoveAt(0);
        }

        if (targets.Count > 0)
        {
            var e = targets.Last();

            Dust.NewDust(e.position, e.width, e.height, DustID.FireworksRGB, newColor: Color.White);
        }

        if (npcs.Count == 0)
        {
            timer2--;

            targets.ForEach(e =>
            {
                Dust.NewDust(e.position, e.width, e.height, DustID.FireworksRGB, newColor: Color.Red);
            });

            if (timer2 == 0)
            {
                targets.ForEach(e =>
                {
                    Projectile.NewProjectile(Terraria.Entity.GetSource_None(), Player.Center, e.position - Player.position, ProjectileID.Bullet, 4, 4, Player.whoAmI);
                });

                targets.Clear();
                RequestedWeapons.Remove(Weapon.ShoulderGun);

                timer = 15;
                timer2 = 60;
            }
        }
    }

    public override void PostUpdate()
    {
        if (!IronManPlayer.IsTransformed) return;

        if (AbilityRequested(Weapon.Repulsor)/* && AbilityUsable<Repulsor>()*/) TryGetAbility<Repulsor>().SpawnRepulsor();
        ShoulderGun();
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (!IronManPlayer.IsTransformed || IronManPlayer.Mark == null || IronManPlayer.Mark == 1 || !IronManPlayer.SuitOn) return;

        if (triggersSet.MouseRight && !Player.mouseInterface && AbilityUsable<Repulsor>())
        {
            if (!AbilityRequested(Weapon.Repulsor) && RepulsorCooldown == 60)
            {
                RepulsorChargeSoundSlot = SoundEngine.PlaySound(Assets.ToSoundStyle(Assets.Sounds.IronMan.RepulsorCharge));
                RequestedWeapons.Add(Weapon.Repulsor);
            }
        }
        else
        {
            if (RepulsorCooldown != 60)
            {
                if (SoundEngine.TryGetActiveSound(RepulsorChargeSoundSlot, out var sound))
                {
                    sound.Stop();
                    SoundEngine.PlaySound(Assets.ToSoundStyle(Assets.Sounds.IronMan.Depower));
                }
            }

            RequestedWeapons.Remove(Weapon.Repulsor);
            RepulsorCooldown = 60;
        }

        if (triggersSet.MouseMiddle && !Player.mouseInterface && AbilityUsable<Repulsor>())
        {
            if (!AbilityRequested(Weapon.Unibeam) && UnibeamCooldown == 180)
            {
                UnibeamChargeSoundSlot = SoundEngine.PlaySound(Assets.ToSoundStyle(Assets.Sounds.IronMan.UnibeamCharge));
                RequestedWeapons.Add(Weapon.Unibeam);
            }
        }
        else
        {
            if (UnibeamCooldown != 180)
            {
                if (SoundEngine.TryGetActiveSound(UnibeamChargeSoundSlot, out var sound))
                {
                    sound.Stop();
                    SoundEngine.PlaySound(Assets.ToSoundStyle(Assets.Sounds.IronMan.Depower));
                }
            }

            RequestedWeapons.Remove(Weapon.Unibeam);
            UnibeamCooldown = 180;
        }

        if (triggersSet.Hotbar1 && !AbilityRequested(Weapon.ShoulderGun)) RequestedWeapons.Add(Weapon.ShoulderGun);
    }
}
