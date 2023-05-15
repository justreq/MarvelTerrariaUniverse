using MarvelTerrariaUniverse.AssetManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace MarvelTerrariaUniverse.Content.Effects;

public class LaserParticle : Particle
{
    float shrinkspd = 0.8f;

    private static Asset<Texture2D> tex = Assets.ToTexture2D(Assets.Textures.Effects.LaserEffect);
    public override void Init()
    {
        texture = (Texture2D)tex;
        frameMax = 1;
        frameDuration = 999;
        frame = new Rectangle(0, 0, texture.Width, texture.Height / frameMax);
        origin = new(0, 16);
        scale.X /= texture.Width;
    }

    public override void Update()
    {
        base.Update();

        scale.Y *= Main.mouseRight || Main.mouseMiddle ? 0f : shrinkspd;
        if (scale.Y <= 0.01f)
            Destroy();
    }

    public override void Draw(SpriteBatch sb)
    {
        base.Draw(sb);
        if (texture != null)
        {
            Draw_Sprite(sb, texture, frame, position, origin, new(scale.X, scale.Y * 2f), rotation, color, 0.2f);
        }
    }
}