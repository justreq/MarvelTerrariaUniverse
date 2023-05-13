using MarvelTerrariaUniverse.AssetManager;
using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.Common.UIElements.SuitModuleHubUI;
using MarvelTerrariaUniverse.Content.TileEntities;
using MarvelTerrariaUniverse.Content.Tiles;
using MarvelTerrariaUniverse.Utilities;
using MarvelTerrariaUniverse.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace MarvelTerrariaUniverse.Common.SuitModuleHubUI;

public static class Colors
{
    public static Color Darkest = new(11, 9, 41);
    public static Color Darker = new(24, 26, 69);
    public static Color Dark = new(39, 46, 97);
    public static Color Medium = new(54, 68, 128);
    public static Color Bright = new(75, 102, 171);
    public static Color Brighter = new(100, 138, 204);
    public static Color Brightest = new(129, 177, 235);
}

public class SuitModuleHubUIState : UIState
{
    public enum State
    {
        Grids = 0,
        Info = 1
    }

    public State CurrentState = State.Grids;
    public static bool Visible { get; set; }

    public List<int?> displayCases = new();

    public UIElement ContentContainer { get; set; }
    public UIPanel SuitGridsPanel { get; set; }
    public UIGrid EquipSuitGrid { get; set; }
    public UIGrid FabricateSuitGrid { get; set; }
    public UIPanel SuitInfoPanel { get; set; }

    public int GridColumnCount = 10;
    public float GridPadding = 12f;
    public float GridElementSize;

    public int? ShowInfoFor = null;

    public override void OnInitialize()
    {
        ContentContainer = this.AddElement(new UIElement().With(e =>
        {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;
            e.Width = StyleDimension.FromPixels(800f);
            e.Height = StyleDimension.FromPixels(480f);
        }));

        SuitGridsPanel = ContentContainer.AddElement(new UIPanel().With(e =>
        {
            e.Width = StyleDimension.Fill;
            e.Height = StyleDimension.Fill;
            e.BorderColor = Color.Black;
            e.BackgroundColor = Colors.Darker * 0.7f;
        }));

        SuitInfoPanel = new UIPanel().With(e =>
        {
            e.Width = StyleDimension.Fill;
            e.Height = StyleDimension.Fill;
            e.BorderColor = Color.Black;
            e.BackgroundColor = Colors.Darker * 0.7f;
        });

        #region Suit Grids Panel

        var suitGridsSearchControlsPanel = SuitGridsPanel.AddElement(new UIGrid().With(e =>
        {
            e.ListPadding = 12f;
            e.Width = StyleDimension.Fill;
            e.Height = StyleDimension.FromPixels(28f);
        }));

        var suitGridsSearchButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search", ReLogic.Content.AssetRequestMode.ImmediateLoad)).With(e =>
        {
            e.VAlign = 0.5f;
            suitGridsSearchControlsPanel.Add(e);
            e.SetVisibility(1f, 1f);
            e.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border", ReLogic.Content.AssetRequestMode.ImmediateLoad));
        });

        var suitGridsSearchBarPanel = new UIPanel().With(e =>
        {
            e.VAlign = 0.5f;
            e.Width = StyleDimension.FromPixels(200f);
            e.Height = StyleDimension.FromPixels(24f);
            e.BorderColor = Color.Black;
            e.BackgroundColor = Colors.Darker * 0.7f;
            suitGridsSearchControlsPanel.Add(e);
        });

        var exitButton = SuitGridsPanel.AddElement(new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel", ReLogic.Content.AssetRequestMode.ImmediateLoad)).With(e =>
        {
            e.HAlign = 1f;
            e.Width = StyleDimension.FromPixels(24f);
            e.Height = StyleDimension.FromPixels(24f);

            e.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) => ToggleVisible(false);
        }));

        var suitGridsContainer = SuitGridsPanel.AddElement(new UIPanel().With(e =>
        {
            e.VAlign = 1f;
            e.Width = StyleDimension.Fill;
            e.Height = StyleDimension.FromPixelsAndPercent(-40f, 1f);
            e.BorderColor = Color.Black;
            e.BackgroundColor = Colors.Darker * 0.7f;
            e.OverflowHidden = true;
        }));

        var SuitGridsContentList = suitGridsContainer.AddElement(new UIList().With(e =>
        {
            e.Width = StyleDimension.Fill;
            e.Height = StyleDimension.Fill;
            e.ListPadding = GridPadding;
        }));

        suitGridsContainer.AddElement(new UIScrollbar().With(e =>
        {
            e.HAlign = 1f;
            e.VAlign = 0.5f;
            e.Height = StyleDimension.FromPixelsAndPercent(-16f, 1f);
            SuitGridsContentList.SetScrollbar(e);
        }));

        SuitGridsContentList.Add(new UIText("Equip Suit"));

        EquipSuitGrid = new UIGrid().With(e =>
        {
            e.Width = StyleDimension.FromPixelsAndPercent(-16f, 1f);
            e.ListPadding = GridPadding;
            SuitGridsContentList.Add(e);
        });

        SuitGridsContentList.Add(new UIText("Fabricate Suit"));

        FabricateSuitGrid = new UIGrid().With(e =>
        {
            e.Width = StyleDimension.FromPixelsAndPercent(-16f, 1f);
            e.ListPadding = GridPadding;
            SuitGridsContentList.Add(e);
        });

        #endregion

        #region Suit Info Panel

        #endregion
    }

    public override void Update(GameTime gameTime)
    {
        if (Main.keyState.IsKeyDown(Keys.Escape)) Visible = false;

        if (ContentContainer.IsMouseHovering)
        {
            PlayerInput.LockVanillaMouseScroll("SuitModuleHubUI");
            Main.LocalPlayer.mouseInterface = true;
        }

        UpdateEquipSuitGrid();
    }

    public void ToggleVisible(bool visible)
    {
        SoundEngine.PlaySound(visible ? SoundID.MenuOpen : SoundID.MenuClose);
        Visible = visible;

        ChangeState(State.Grids);
    }

    public void UpdateEquipSuitGrid()
    {
        if (Main.gameMenu) return;

        List<int?> foundTiles = new();
        List<Point16> checkedTiles = new();

        var suitModuleHubPosition = Main.LocalPlayer.GetModPlayer<IronManPlayer>().LastAccessedSuitModuleHubPosition;

        for (int i = (int)suitModuleHubPosition.X - 14; i < suitModuleHubPosition.X + 15; i++)
        {
            for (int j = (int)suitModuleHubPosition.Y - 14; j < suitModuleHubPosition.Y + 15; j++)
            {
                var origin = TileUtils.GetTileOrigin(i, j);

                if (Framing.GetTileSafely(i, j).TileType != ModContent.TileType<DisplayCase>() || checkedTiles.Contains(origin)) continue;

                var entity = (DisplayCaseTileEntity)TileEntity.ByID[ModContent.GetInstance<DisplayCaseTileEntity>().Find(origin.X, origin.Y)];

                checkedTiles.Add(origin);
                foundTiles.Add(entity.mark);
            }
        }

        if (!displayCases.SequenceEqual(foundTiles))
        {
            EquipSuitGrid.Clear();

            GridElementSize = EquipSuitGrid.GetOuterDimensions().Width / GridColumnCount - GridPadding;
            foundTiles.ForEach(i =>
            {
                EquipSuitGrid.Add(new SuitGridElement(GridElementSize, i).With(e =>
                {
                    e.associatedDisplayCase = checkedTiles[foundTiles.IndexOf(i)];
                    e.OnLeftClick += EquipButtonLeftClick;
                    e.OnRightClick += EquipButtonRightClick;
                }));
            });

            EquipSuitGrid.Height = StyleDimension.FromPixels((float)Math.Ceiling((double)((double)foundTiles.Count / GridColumnCount)) * (GridPadding + GridElementSize));

            FabricateSuitGrid.Clear();

            for (int i = 1; i <= 7; i++)
            {
                FabricateSuitGrid.Add(new SuitGridElement(GridElementSize, i).With(e =>
                {
                    e.OnLeftClick += FabricateButtonClick;
                }));
            }

            FabricateSuitGrid.Height = StyleDimension.FromPixels((float)Math.Ceiling((double)((double)FabricateSuitGrid.Count / GridColumnCount)) * (GridPadding + GridElementSize));
        }

        displayCases = foundTiles;
    }

    public void EquipButtonLeftClick(UIMouseEvent evt, UIElement listeningElement)
    {
        var target = (SuitGridElement)listeningElement;
        var ironManPlayer = Main.LocalPlayer.GetModPlayer<IronManPlayer>();

        if (target.mark == null)
        {
            if (ironManPlayer.Mark != null)
            {
                var origin = TileUtils.GetTileOrigin(target.associatedDisplayCase.Value.X, target.associatedDisplayCase.Value.Y);
                var entity = (DisplayCaseTileEntity)TileEntity.ByID[ModContent.GetInstance<DisplayCaseTileEntity>().Find(origin.X, origin.Y)];

                entity.mark = ironManPlayer.Mark;
                ironManPlayer.EquipSuit();
            }

            return;
        }

        ChangeState(State.Info, target.mark);

    }

    public void EquipButtonRightClick(UIMouseEvent evt, UIElement listeningElement)
    {
        var target = (SuitGridElement)listeningElement;
        if (target.mark == null) return;

        EquipSuitRequested(target);
    }

    public void EquipSuitRequested(SuitGridElement associatedGridButton)
    {
        var origin = TileUtils.GetTileOrigin(associatedGridButton.associatedDisplayCase.Value.X, associatedGridButton.associatedDisplayCase.Value.Y);
        var entity = (DisplayCaseTileEntity)TileEntity.ByID[ModContent.GetInstance<DisplayCaseTileEntity>().Find(origin.X, origin.Y)];
        entity.mark = null;

        Main.LocalPlayer.GetModPlayer<IronManPlayer>().EquipSuit(associatedGridButton.mark);
        ToggleVisible(false);
    }

    public void FabricateButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
        var target = (SuitGridElement)listeningElement;

        ChangeState(State.Info, target.mark);
    }

    public void ChangeState(State state, int? mark = null)
    {
        if (state == CurrentState) return;

        CurrentState = CurrentState == State.Grids ? State.Info : State.Grids;

        if (CurrentState != State.Grids)
        {
            SuitGridsPanel.Remove();
            ContentContainer.Append(SuitInfoPanel);
            ShowInfoFor = mark;

            SuitInfoPanel.AddElement(new SuitBanner((int)mark));
        }
        else
        {
            SuitInfoPanel.RemoveAllChildren();
            SuitInfoPanel.Remove();
            ContentContainer.Append(SuitGridsPanel);
            ShowInfoFor = null;
        }
    }
}
