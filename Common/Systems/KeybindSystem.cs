using Terraria.ModLoader;

public class KeybindSystem : ModSystem
{
    public static ModKeybind ToggleFaceplate;
    public static ModKeybind ToggleFlight;
    public static ModKeybind EjectSuit;

    public static void RegisterKeybindWithCategory(ref ModKeybind variableSavedTo, Mod mod, string category, string name, string defaultBinding)
    {
        variableSavedTo = KeybindLoader.RegisterKeybind(mod, name, defaultBinding);
        MarvelTerrariaUniverse.MarvelTerrariaUniverse.CategorizedModKeybinds.Add(name, category);
    }

    public override void Load()
    {
        RegisterKeybindWithCategory(ref ToggleFaceplate, Mod, "Iron Man", "ToggleFaceplate", "G");
        RegisterKeybindWithCategory(ref ToggleFlight, Mod, "Iron Man", "ToggleFlight", "F");
        RegisterKeybindWithCategory(ref EjectSuit, Mod, "Iron Man", "EjectSuit", "X");
    }

    public override void Unload()
    {
        ToggleFaceplate = null;
        ToggleFlight = null;
        EjectSuit = null;
    }
}