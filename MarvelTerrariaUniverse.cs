using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using MarvelTerrariaUniverse.Common.Net;

namespace MarvelTerrariaUniverse;
public class MarvelTerrariaUniverse : Mod
{
    public static MarvelTerrariaUniverse Instance => ModContent.GetInstance<MarvelTerrariaUniverse>();
    public enum Transformation
    {
        None = 0,
        IronMan = 1
    }

    public static Dictionary<string, string> CategorizedModKeybinds = new(); // unused for now... will be used when lolxd and i figure out the fuckery behind the keybind menu soontm

    public const int IRONMANSUITS = 7;
    public const int EXTRALOADOUTS = 1;

    public static readonly Color[,] CustomLoadoutColors = new Color[EXTRALOADOUTS, 3] {
        { new(186, 12, 47), new(155, 17, 30), new(143, 7, 15) }
    };

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        MTUNetMessages.HandlePacket(reader, whoAmI);
    }
}