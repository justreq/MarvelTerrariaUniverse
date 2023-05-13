using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Items.Placeable
{
    public class DisplayCase : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DisplayCase>());
            Item.width = 48;
            Item.height = 80;
        }
    }
}