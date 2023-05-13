using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.TileEntities;

public abstract class DrawableTileEntity : ModTileEntity
{
    protected virtual Point Size => Point.Zero;

    protected Vector2 World => Position.ToWorldCoordinates();

    internal abstract void Draw(SpriteBatch draw);

    public virtual bool CanDraw() => new Rectangle((int)(World.X - Main.screenPosition.X), (int)(World.Y - Main.screenPosition.Y), Size.X, Size.Y).Intersects(new Rectangle(0, 0, Main.screenWidth, Main.screenHeight));
}
