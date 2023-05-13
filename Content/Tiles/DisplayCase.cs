using MarvelTerrariaUniverse.Content.Items.IronMan;
using MarvelTerrariaUniverse.Content.TileEntities;
using MarvelTerrariaUniverse.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MarvelTerrariaUniverse.Content.Tiles
{
    public class DisplayCase : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            AddMapEntry(new Color(173, 216, 230));
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.AnchorAlternateTiles = new[] { (int)Type };
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {

            var origin = TileUtils.GetTileOrigin(i, j);
            var entity = (DisplayCaseTileEntity)TileEntity.ByID[ModContent.GetInstance<DisplayCaseTileEntity>().Find(origin.X, origin.Y)];

            return entity.mark == null;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            var origin = TileUtils.GetTileOrigin(i, j);
            ModContent.GetInstance<DisplayCaseTileEntity>().Place(origin.X, origin.Y);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 3 * 16, 4 * 16, ModContent.ItemType<Items.Placeable.DisplayCase>());
            ModContent.GetInstance<DisplayCaseTileEntity>().Kill(i, j);
        }

        public override bool RightClick(int i, int j)
        {
            var tileCoords = Main.MouseWorld.ToTileCoordinates();
            var origin = TileUtils.GetTileOrigin(tileCoords.X, tileCoords.Y);
            var entity = (DisplayCaseTileEntity)TileEntity.ByID[ModContent.GetInstance<DisplayCaseTileEntity>().Find(origin.X, origin.Y)];
            var player = Main.LocalPlayer;

            if (entity.mark != null && player.position.WithinRange(Main.MouseWorld, 4 * 16f))
            {
                int type = SuitModuleItem.ItemTypeByMark((int)entity.mark);
                player.QuickSpawnItem(player.GetSource_TileInteraction(i, j), type);
                entity.mark = null;
            }

            return true;
        }
    }
}