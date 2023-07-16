using MarvelTerrariaUniverse.Content.Items.IronMan;
using MarvelTerrariaUniverse.Core.IronMan;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse;

public class MarvelTerrariaUniverse : Mod
{
    public enum Transformation
    {
        None = 0,
        IronMan = 1
    }

    public const int IronManSuitMarkCount = 7;

    public static Dictionary<List<string>, EquipType> TransformationTextures = new();
    public static HashSet<string> TransformationTypes = new();

    public override void Load()
    {
        GetFileNames().Where(e => e.StartsWith("Assets/Textures/Transformations")).ToList().ForEach(file =>
        {
            var root = "Assets/Textures/Transformations/";
            var path = file[root.Length..].Split("/");
            var type = path.First(e => e.Contains(".rawimg")).Split(".")[0];
            var name = path.Length == 3 ? $"{path[0]}{path[1]}" : path[1];

            if (type.Contains("Alt"))
            {
                name += $"Alt{(type.Split("Alt").Length == 2 ? type.Split("Alt")[1] : "")}";
                type = type.Split("Alt")[0];

            }

            EquipLoader.AddEquipTexture(this, $"MarvelTerrariaUniverse/{file}".Split(".")[0], Enum.Parse<EquipType>(type), name: name);
            TransformationTextures.Add(new() { name }, Enum.Parse<EquipType>(type));
            TransformationTypes.Add(name);
        });

        for (int i = 1; i <= IronManSuitMarkCount; i++)
        {
            AddContent(new SuitModuleItem(i));
        }
    }
}