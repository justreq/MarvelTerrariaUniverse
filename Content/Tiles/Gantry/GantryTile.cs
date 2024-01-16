using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using MarvelTerrariaUniverse.Content.Items.Tiles;

namespace MarvelTerrariaUniverse.Content.Tiles.Gantry
{
    public class GantryTile : ModTile //https://github.com/tModLoader/tModLoader/blob/1.4.4/ExampleMod/Content/Tiles/Furniture/ExampleChest.cs
    {
        //private void equipSuit()
        //{
        //    var ironManPlayer = Main.LocalPlayer.GetModPlayer<IronManPlayer>();
        //    ModTile tile = ModContent.GetModTile(ModContent.TileType<GantryTile>());
        //    //loop through all chests in the world until we find the one we are looking for
        //    if (ironManPlayer != null)
        //    {
        //        foreach (var chest in Main.chest)
        //        {
        //            if (chest.Equals(tile)) { }
        //            if (chest.item[0].Name == null)
        //            {
        //                return;
        //            }
        //        }   
        //    }
        //} 

        public override void SetStaticDefaults()
        {
            Main.tileContainer[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            //TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.BasicChest[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.AvoidedByNPCs[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;
            //TileID.Sets.IsAContainer[Type] = true;
            AdjTiles = new int[] { TileID.Containers }; //what type of item when player looking for nearby crafting station

            AddMapEntry(new Color(200, 200, 200), this.GetLocalization("MapEntry0"));
            AddMapEntry(new Color(0, 141, 63), this.GetLocalization("MapEntry1"));

            RegisterItemDrop(ModContent.ItemType<GantryTileItem>(), 1);
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new int[] {
                TileID.MagicalIceBlock,
                TileID.Boulder,
                TileID.BouncyBoulder,
                TileID.LifeCrystalBoulder,
                TileID.RollingCactus
            };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            Main.placementPreview = true;
        }
        //public override void PlaceInWorld(int i, int j, Item item)
        //{
        //    WorldGen.PoundTile(i - 2, j);
        //    WorldGen.PoundTile(i - 1, j);
        //    WorldGen.PoundTile(i, j);
        //    WorldGen.PoundTile(i + 1, j);
        //}
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // We override KillMultiTile to handle additional logic other than the item drop. In this case, unregistering the Chest from the world
            Chest.DestroyChest(i, j);
        }

        public bool PlayGantryFrames =false;
        public int GantryFrame = 0;
  

        int Timer = 0;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            Timer++;

            if (Timer > 3)
            {
                if (PlayGantryFrames)
                {
                    if (GantryFrame < 40) GantryFrame++; //46 //mine was 45
                }
                else
                {
                    if (GantryFrame > 0) GantryFrame--;
                }

                Timer = 0;
            }

            if (Framing.GetTileSafely(i - 2, j).BlockType != BlockType.Solid && Framing.GetTileSafely(i + 1, j).BlockType != BlockType.Solid)                                                                //52                         //68                  //68
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("MarvelTerrariaUniverse/Content/Tiles/Gantry/GantryFrames").Value, new Vector2(i * 16 - (int)Main.screenPosition.X - 48, j * 16 - (int)Main.screenPosition.Y - 48) + zero, new Rectangle(0, 66 * GantryFrame, 96, 66), Color.White);
            }
        }

       
        public override bool RightClick(int i, int j)
        {
            // Get references to the player and the tile at the specified coordinates
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];

            // Adjust coordinates based on the tile frame to ensure accurate handling of the chest
            int left = i;
            int top = j;


            // Adjust for horizontal offset based on the origin
            if ((tile.TileFrameX) != 0)
            {
                left--;
            }

            // Adjust for vertical offset based on the origin
            if (tile.TileFrameY != 0)
            {
                top--;
            }
            // Clear various UI elements to provide a clean interaction experience
            player.CloseSign();
            player.SetTalkNPC(-1);
            Main.npcChatCornerItem = 0;
            Main.npcChatText = "";

            // Handle chest editing mode
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }

            // Sync edited chest name in multiplayer
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
                player.editedChestName = false;
            }


            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Multiplayer logic for chest interaction
                if (left == player.chestX && top == player.chestY && player.chest != -1)
                {
                    // Close the chest if already open
                    player.chest = -1;
                    Recipe.FindRecipes();
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    // Request to open the chest in multiplayer
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                // Handle opening or closing of the chest in singleplayer
                int chest = Chest.FindChest(left, top);
                if (chest != -1)
                {
                    Main.stackSplit = 600;
                    if (chest == player.chest)
                    {
                        // Close the chest if already open
                        player.chest = -1;
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    else
                    {
                        // Open the chest and play appropriate sounds
                        SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                        player.OpenChest(left, top, chest);
                    }

                    Recipe.FindRecipes();

                }
            }

            // Indicate that the right-click interaction was handled
            return true;
        }
    }
}
