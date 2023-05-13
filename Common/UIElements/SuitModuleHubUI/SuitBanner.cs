using MarvelTerrariaUniverse.Utilities.Extensions;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace MarvelTerrariaUniverse.Common.UIElements.SuitModuleHubUI;
public class SuitBanner : UIPanel
{
    public int mark;

    public SuitBanner(int mark) : base()
    {
        this.mark = mark;

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

        this.AddElement(new UITransformationCharacter($"IronManMark{mark}").With(e =>
        {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;
        }));
    }
}
