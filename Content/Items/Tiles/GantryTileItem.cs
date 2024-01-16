using MarvelTerrariaUniverse.Content.Tiles.Gantry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Items.Tiles
{
    public class GantryTileItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.noUseGraphic = false; //true
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.width = 56; //96
            Item.height = 42; //74 //66
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<GantryTile>();
        }


    }
}
