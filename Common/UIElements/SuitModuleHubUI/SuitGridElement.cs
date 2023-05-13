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

        if (mark == null) return;

        suitPreview = this.AddElement(new UITransformationCharacter($"IronManMark{mark}").With(e =>
        {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;
        }));
    }

    public override int CompareTo(object obj)
    {
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
            BorderColor = Main.OurFavoriteColor;
            if (mark != null) Main.hoverItemName = $"Mark {mark}{(alias == null ? "" : $" \"{alias}\"")}";
        }
        else BorderColor = Color.Black;

        if (suitPreview == null) return;

        if (mark != 1)
        {
            HelmetGlowmask.RegisterData(EquipLoader.GetEquipSlot(mod, suitPreview.transformation, EquipType.Head), new DrawLayerData()
            {
                Texture = Assets.ToTexture2D(Assets.Textures.Glowmasks.IronMan.Faceplate0),
                Color = (drawInfo) => Color.White
            });
        }

        // BasePlayer.RegisterData(EquipLoader.GetEquipSlot(mod, suitPreview.transformation, EquipType.Body), () => Color.White);
    }
}
