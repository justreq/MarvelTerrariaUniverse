using MarvelTerrariaUniverse.Content.TileEntities;
using MarvelTerrariaUniverse.Content.Tiles;
using MarvelTerrariaUniverse.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Items.IronMan;

[Autoload(false)]
public class SuitModuleItem : ModItem
{
    internal static Dictionary<int, SuitModuleItem> suitsByMark = new();

    internal static int ItemTypeByMark(int mark) => suitsByMark.TryGetValue(mark, out var item) ? item.Type : throw new System.Exception("Blyat not valid type");

    protected override bool CloneNewInstances => true;

    public override string Name => base.Name.Replace("SuitModuleItem", "SuitModule") + mark;

    public override string Texture => base.Texture;

    public int mark;
    public SuitModuleItem(int mark)
    {
        this.mark = mark;
    }

    public override LocalizedText DisplayName => Language.GetText("Mods.MarvelTerrariaUniverse.Common.IronManSuitModule.DisplayName").WithFormatArgs(mark);

    public override LocalizedText Tooltip => Language.GetText("Mods.MarvelTerrariaUniverse.Common.IronManSuitModule.Tooltip").WithFormatArgs(mark);

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
        suitsByMark[mark] = this;
    }

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 22;
        Item.maxStack = 1;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Thrust;
    }

    public override bool? UseItem(Player player)
    {
        var location = Main.MouseWorld.ToTileCoordinates();

        if (Framing.GetTileSafely(Main.MouseWorld.ToTileCoordinates()).TileType != ModContent.TileType<DisplayCase>() || !player.position.WithinRange(Main.MouseWorld, 4 * 16f)) return false;

        var origin = TileUtils.GetTileOrigin(location.X, location.Y);
        var entity = (DisplayCaseTileEntity)TileEntity.ByID[ModContent.GetInstance<DisplayCaseTileEntity>().Find(origin.X, origin.Y)];
        var markToChange = entity.mark;

        entity.mark = mark;
        entity.direction = player.direction;

        if (markToChange != null)
        {
            int type = ItemTypeByMark((int)markToChange);
            player.HeldItem.SetDefaults(type);
        }
        else
        {
            player.HeldItem.TurnToAir();
        }
        return true;
    }
}
