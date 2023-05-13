using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Common.Systems;
public class KeybindSystem : ModSystem
{
    public static ModKeybind ToggleFlight { get; set; }
    public static ModKeybind ToggleFaceplate { get; set; }
    public static ModKeybind DropHelmet { get; set; }
    public static ModKeybind EjectSuit { get; set; }

    public override void Load()
    {
        ToggleFlight = KeybindLoader.RegisterKeybind(Mod, "ToggleFlight", "F");
        ToggleFaceplate = KeybindLoader.RegisterKeybind(Mod, "ToggleFaceplate", "G");
        DropHelmet = KeybindLoader.RegisterKeybind(Mod, "DropHelmet", "H");
        EjectSuit = KeybindLoader.RegisterKeybind(Mod, "EjectSuit", "X");
    }

    public override void Unload()
    {
        ToggleFlight = null;
        ToggleFaceplate = null;
        DropHelmet = null;
        EjectSuit = null;
    }
}
