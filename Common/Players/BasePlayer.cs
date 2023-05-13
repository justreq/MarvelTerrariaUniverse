using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace MarvelTerrariaUniverse.Common.Players;
public class BasePlayer : ModPlayer
{
    public Transformations transformation = Transformations.None;

    private static Dictionary<int, Func<Color>> BodyColor { get; set; }

    public static void RegisterData(int bodySlot, Func<Color> color)
    {
        if (!BodyColor.ContainsKey(bodySlot)) BodyColor.Add(bodySlot, color);
    }

    public override void Load()
    {
        BodyColor = new Dictionary<int, Func<Color>>();
    }

    public override void Unload()
    {
        BodyColor = null;
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (!BodyColor.TryGetValue(Player.body, out Func<Color> color)) return;

        drawInfo.bodyGlowColor = color();
        drawInfo.armGlowColor = color();
    }

    public override void SaveData(TagCompound tag)
    {
        tag["transformation"] = (int)transformation;
    }

    public override void LoadData(TagCompound tag)
    {
        transformation = (Transformations)(int)tag["transformation"];
    }
}