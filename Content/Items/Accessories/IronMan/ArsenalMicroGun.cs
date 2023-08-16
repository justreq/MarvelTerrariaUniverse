using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ID;
using ReLogic.Utilities;
using System.Linq;
using Terraria.ModLoader.IO;

namespace MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
public class ArsenalMicroGun : ArsenalItem
{
    //Player player1 = Main.player[Main.myPlayer];
    //public List<NPC> GetNearestXNPCs(float maxDistance, int count)
    //{
    //    return Main.npc.SkipLast(1).Where(e => e.active && e.WithinRange(player1.Center, maxDistance)).OrderBy(e => e.DistanceSQ(player1.Center)).Take(count).ToList();
    //}

    //List<NPC> npcs = new();
    //List<NPC> targets = new();
    //int timer = 15;
    //int timer2 = 60;

    //public override void UpdateArsenal(Player player)
    //{
        
    //}

    //public override void UpdateAccessory(Player player, bool hideVisual)
    //{
    //    base.UpdateAccessory(player, hideVisual);

    //    if (npcs.Count == 0 && timer2 == 60)
    //    {
    //        npcs = GetNearestXNPCs(750, 12);

    //        if (npcs.Count == 0)
    //        {
    //            return;
    //        }
    //    }
    //    timer--;
    //    if (timer == 0 && npcs.Count > 0)
    //    {
    //        timer = 15;

    //        targets.Add(npcs.First());
    //        npcs.RemoveAt(0);
    //    }

    //    if (targets.Count > 0)
    //    {
    //        var e = targets.Last();

    //        Dust.NewDust(e.position, e.width, e.height, DustID.FireworksRGB, newColor: Color.White);
    //    }

    //    if (npcs.Count == 0)
    //    {
    //        timer2--;

    //        targets.ForEach(e =>
    //        {
    //            Dust.NewDust(e.position, e.width, e.height, DustID.FireworksRGB, newColor: Color.Red);
    //        });

    //        if (timer2 == 0)
    //        {
    //            targets.ForEach(e =>
    //            {
    //                Projectile.NewProjectile(Terraria.Entity.GetSource_None(), player.Center, e.position - player.position, ProjectileID.Bullet, 4, 4, player.whoAmI);
    //            });

    //            targets.Clear();
      

    //            timer = 15;
    //            timer2 = 60;
    //        }
    //    }
    //}
}
