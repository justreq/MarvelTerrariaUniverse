using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Items.Placeable
{
    public class SuitModuleHub : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SuitModuleHub>());
            Item.width = 48;
            Item.height = 32;
        }
    }
}