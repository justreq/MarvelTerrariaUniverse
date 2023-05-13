using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MarvelTerrariaUniverse.Common.SuitModuleHubUI;
public class SuitModuleHubUISystem : ModSystem
{
    public UserInterface SuitModuleHubUserInterface;
    private GameTime lastUpdateUiGameTime;

    public override void Load()
    {
        LoadUI();
    }

    public void LoadUI()
    {
        if (!Main.dedServ)
        {
            SuitModuleHubUIState SuitModuleHubUI = new();
            SuitModuleHubUserInterface = new UserInterface();
            SuitModuleHubUserInterface.SetState(SuitModuleHubUI);
            SuitModuleHubUI.Activate();
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        lastUpdateUiGameTime = gameTime;
        if (SuitModuleHubUIState.Visible) SuitModuleHubUserInterface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("MarvelTerrariaUniverse: Suit Module Hub User Interface", delegate
            {
                if (lastUpdateUiGameTime != null && SuitModuleHubUIState.Visible) SuitModuleHubUserInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);

                return true;
            }, InterfaceScaleType.UI));
        }
    }
}
