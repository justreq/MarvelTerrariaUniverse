using MarvelTerrariaUniverse.AssetManager;
using MarvelTerrariaUniverse.Common.PlayerLayers.IronMan;
using MarvelTerrariaUniverse.Common.PlayerLayers;
using MarvelTerrariaUniverse.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using MarvelTerrariaUniverse.Common.SuitModuleHubUI;
using Terraria.DataStructures;
using MarvelTerrariaUniverse.Common.Players;

namespace MarvelTerrariaUniverse.Common.UIElements.SuitModuleHubUI;
public class SuitGridElement : UIPanel
{
    public UITransformationCharacter suitPreview;

    public float size;
    public int? mark;
    public string alias;

    public Point16? associatedDisplayCase = null;

    public UIPanel Border { get; set; }

    public SuitGridElement(float size, int? mark, string alias = null) : base(Assets.ToTexture2D(Assets.Textures.UI.SuitGridElementPanelBackground), Assets.ToTexture2D(Assets.Textures.UI.SuitGridElementPanelBorder))
    {
        this.size = size;
        this.mark = mark;
        this.alias = alias;

        Width = StyleDimension.FromPixels(size);
        Height = StyleDimension.FromPixels(size);
        BackgroundColor = Colors.Medium * 0.7f;
        BorderColor = Color.Black;
        OverflowHidden = true;
        SetPadding(0f);

        if (mark != null)
        {
            this.AddElement(new UIText(mark.ToString(), 0.8f).With(e =>
            {
                e.Left = StyleDimension.FromPixels(5);
                e.Top = StyleDimension.FromPixels(5);
            }));

            suitPreview = this.AddElement(new UITransformationCharacter($"IronManMark{mark}", 2f).With(e =>
            {
                e.HAlign = 0.5f;
                e.VAlign = 0.5f;
                e.Left = StyleDimension.FromPixels(-13f);
                e.Top = StyleDimension.FromPixels(-6f);
            }));
        }

        Border = this.AddElement(new UIPanel(Assets.ToTexture2D(Assets.Textures.UI.SuitGridElementPanelBackground), Assets.ToTexture2D(Assets.Textures.UI.SuitGridElementPanelBorder)).With(e =>
        {
            e.Width = StyleDimension.FromPixels(size);
            e.Height = StyleDimension.FromPixels(size);
            e.BackgroundColor = Color.Transparent;
            e.BorderColor = Color.Black;
            e.OverflowHidden = true;
            e.SetPadding(0f);
        }));
    }

    public override int CompareTo(object obj)
    {
        // expectation: if null, put it ahead of everything else, otherwise order it in ascending order
        SuitGridElement other = obj as SuitGridElement;
        if (mark == null || other.mark == null) return -1;
        return ((int)mark).CompareTo((int)other.mark);
    }

    Mod mod = ModContent.GetInstance<MarvelTerrariaUniverse>();

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        if (IsMouseHovering)
        {
            Border.BorderColor = Main.OurFavoriteColor;
            Main.hoverItemName = mark != null ? $"Mark {mark}{(alias == null ? "" : $" \"{alias}\"")}" : Main.LocalPlayer.GetModPlayer<IronManPlayer>().Mark != null ? "Unequip suit" : "";
        }
        else Border.BorderColor = Color.Black;
    }
}
