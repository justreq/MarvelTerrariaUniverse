using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.Common.SuitModuleHubUI;
using MarvelTerrariaUniverse.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MarvelTerrariaUniverse.Content.Tiles
{
    public class SuitModuleHub : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileBlockLight[Type] = true;
            Main.tileNoAttach[Type] = true;
            AddMapEntry(new Color(173, 216, 230));
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new(2, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.addTile(Type);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 5 * 16, 3 * 16, ModContent.ItemType<Items.Placeable.SuitModuleHub>());
        }

        public override bool RightClick(int i, int j)
        {
            Main.playerInventory = false;
            Main.editChest = false;
            Main.npcChatText = "";
            Main.ClosePlayerChat();
            Main.chatText = "";
            SoundEngine.PlaySound(SoundID.MenuOpen);
            SuitModuleHubUIState.Visible = true;

            Main.LocalPlayer.GetModPlayer<IronManPlayer>().LastAccessedSuitModuleHubPosition = TileUtils.GetTileOrigin(i, j).ToVector2() + new Vector2(2, 1);

            return true;
        }
    }
}