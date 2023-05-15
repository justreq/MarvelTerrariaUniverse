using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using MarvelTerrariaUniverse.Content.Tiles;
using Terraria.ModLoader.IO;
using MarvelTerrariaUniverse.Utilities.Extensions;

namespace MarvelTerrariaUniverse.Content.TileEntities;

public class DisplayCaseTileEntity : DrawableTileEntity
{
    public override bool IsTileValidForEntity(int x, int y)
    {
        Tile tile = Main.tile[x, y];
        return tile.HasTile && tile.TileType == ModContent.TileType<DisplayCase>();
    }

    public static Player drawPlayer = (Player)new Player().Clone();

    public int? mark = null;
    public int direction = 1;

    internal override void Draw(SpriteBatch spriteBatch)
    {
        if (mark == null) return;

        drawPlayer.head = EquipLoader.GetEquipSlot(Mod, $"IronManMark{mark}", EquipType.Head);
        drawPlayer.body = EquipLoader.GetEquipSlot(Mod, $"IronManMark{mark}", EquipType.Body);
        drawPlayer.legs = EquipLoader.GetEquipSlot(Mod, $"IronManMark{mark}", EquipType.Legs);
        drawPlayer.direction = direction;

        Main.PlayerRenderer.DrawPlayer(Main.Camera, drawPlayer, Position.ToWorldCoordinates() - new Vector2(-6f, -10f), 0f, drawPlayer.Hitbox.Size() / 2);
    }

    public override void SaveData(TagCompound tag)
    {
        tag.SaveNullable("mark", mark);
        tag["direction"] = direction;
    }

    public override void LoadData(TagCompound tag)
    {
        mark = tag.LoadNullable<int>("mark");
        direction = (int)tag["direction"];
    }
}