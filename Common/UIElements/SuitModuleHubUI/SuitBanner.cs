using MarvelTerrariaUniverse.AssetManager;
using MarvelTerrariaUniverse.Common.SuitModuleHubUI;
using MarvelTerrariaUniverse.Utilities.Extensions;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace MarvelTerrariaUniverse.Common.UIElements.SuitModuleHubUI;
public class SuitBanner : UIPanel
{
    public List<UIHoverImageButton> cycleSuitInfoButtons = new();

    public int mark;
    public bool newSuit;

    public SuitBanner(int mark, bool newSuit) : base()
    {
        this.mark = mark;
        this.newSuit = newSuit;

        Width = StyleDimension.FromPixels(200f);
        Height = StyleDimension.FromPixels(200f);

        var nameText = this.AddElement(new UIText(Language.GetText("Mods.MarvelTerrariaUniverse.IronMan.Name").WithFormatArgs(mark)));

        if (Language.Exists($"Mods.MarvelTerrariaUniverse.IronMan.{mark}.Alias"))
        {
            this.AddElement(new UIText(Language.GetText($"Mods.MarvelTerrariaUniverse.IronMan.{mark}.Alias")).With(e =>
            {
                e.Top = StyleDimension.FromPixels(nameText.GetOuterDimensions().Height + 6f);
            }));
        }

        /*this.AddElement(new UITransformationCharacter($"IronManMark{mark}", 2.5f).With(e =>
        {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;
            e.Left = StyleDimension.FromPixels(3 * -8f);
            e.Top = StyleDimension.FromPixels(3 * -12f);
        }));*/

        if (newSuit)
        {
            for (int i = 0; i < 2; i++)
            {
                var texture = i == 0 ? Assets.Textures.UI.ButtonPrevious : Assets.Textures.UI.ButtonNext;
                var preceder = mark == 1 ? MarvelTerrariaUniverse.IronManSuitMarkCount : mark - 1;
                var successor = mark == MarvelTerrariaUniverse.IronManSuitMarkCount ? 1 : mark + 1;

                var hoverText = $"{(i == 0 ? mark == 1 ? "" : "Preceded by" : mark == MarvelTerrariaUniverse.IronManSuitMarkCount ? "" : "Succeeded by")} Mark {(i == 0 ? preceder : successor)}";

                cycleSuitInfoButtons.Add(this.AddElement(new UIHoverImageButton(Assets.ToTexture2D(texture), hoverText).With(e =>
                 {
                     e.VAlign = 0.5f;
                     e.HAlign = i;
                 })));
            }
        }
    }
}
