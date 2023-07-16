using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.Common.SuitModuleHubUI;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.UI;

namespace MarvelTerrariaUniverse.Utilities;

// EVERYTHING HERE IS FOR DEBUGGING PURPOSES AND WILL NOT BE ACCESSIBLE IN ANY PUBLICLY AVAILABLE BUILDS OF THE MOD :)
// DONT TOUCH ANYTHING HERE BTW PLS THX BYE LUV YA BYE <3

#if DEBUG
class ReloadUISystem : ModSystem
{
    public static ModKeybind ReloadUI { get; private set; }

    public override void Load()
    {
        ReloadUI = KeybindLoader.RegisterKeybind(Mod, "ReloadUI", "R");
    }

    public override void Unload()
    {
        ReloadUI = null;
    }
}

class ReloadUIPlayer : ModPlayer
{
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        var system = ModContent.GetInstance<SuitModuleHubUISystem>();

        if (ReloadUISystem.ReloadUI.JustPressed && !Main.dedServ) system.LoadUI();
    }
}

class AutoJoinWorldSystem : ModSystem
{
    public override void Load()
    {

        MonoModHooks.Add(typeof(ModContent).Assembly.GetType("Terraria.ModLoader.Core.ModOrganizer")!.GetMethod("SaveLastLaunchedMods", BindingFlags.NonPublic | BindingFlags.Static)!, (Action orig) =>
        {
            orig();
            enterCharacterSelectMenu = true;
        });

        On_Main.DrawMenu += Main_DrawMenu;
    }

    static bool enterCharacterSelectMenu;
    private void Main_DrawMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gameTime)
    {
        orig(self, gameTime);
        if (enterCharacterSelectMenu)
        {
            enterCharacterSelectMenu = false;
            Main.OpenCharacterSelectUI();

            var player = Main.PlayerList.First(d => d.Name == "Mod Testing");
            Main.SelectPlayer(player);

            Main.OpenWorldSelectUI();
            UIWorldSelect worldSelect = (UIWorldSelect)typeof(Main).GetField("_worldSelectMenu", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)!.GetValue(null!)!;
            UIList uiList = (UIList)typeof(UIWorldSelect).GetField("_worldList", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!.GetValue(worldSelect)!;
            var item = uiList._items.OfType<UIWorldListItem>().First(d =>
            {
                return ((WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(d)!).Name == "Mod Testing";
            });
            typeof(UIWorldListItem).GetMethod("PlayGame", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!.Invoke(item, new object[]
            {
                    new UIMouseEvent(item, item.GetOuterDimensions().Position()), item
            });
        }
    }
}

class EquipCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;
    public override string Command => "im";
    public override string Usage => "/im <mark>";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        var basePlayer = caller.Player.GetModPlayer<BasePlayer>();
        var ironManPlayer = caller.Player.GetModPlayer<IronManPlayer>();

        if (args.Length == 0) ironManPlayer.EquipSuit();
        else
        {
            if (!int.TryParse(args[0], out int _))
            {
                Main.NewText("Mark required as integer");
                return;
            }

            ironManPlayer.EquipSuit(int.Parse(args[0]));
        }

        Main.NewText($"{basePlayer.transformation} {ironManPlayer.Mark}");
    }
}
#endif