using MarvelTerrariaUniverse.Core.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace MarvelTerrariaUniverse.Content.Effects;

public abstract class Particle : ParticleEntity
{
    public const float MaxParticleDistance = 3000f;
    public const float MaxParticleDistanceSqr = MaxParticleDistance * MaxParticleDistance;

    public float alpha = 1f;

    public float rotation;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 velocityScale = Vector2.One;
    public Vector2 scale = Vector2.One;
    public Vector2 gravity = new(0f, 0f);
    public Vector2 friction;
    public float velocityDecay = 1;
    public Color color = Color.White;
    public Texture2D? texture;
    public Rectangle frame;
    public int frameCount;
    public int frameDuration;
    public int frameIndex;
    public int frameMax;
    public Vector2 origin;

    public int LifeTime { get; private set; }

    public override void Update()
    {

        if (++frameCount >= frameDuration && texture != null)
        {
            if (texture.Height > texture.Width)
                frame.Y += texture.Height / frameMax;
            else
                frame.X += texture.Width / frameMax;
            frameCount = 0;
            if (++frameIndex >= frameMax)
                Destroy();
        }
        velocity += gravity;
        velocity -= friction;
        velocity *= velocityDecay;


        position += velocity * velocityScale;
        LifeTime++;
    }

    public Vector2 origin_center(Texture2D tex)
    {
        return new Vector2(tex.Width / 2, (tex.Height / frameMax) / 2);
    }

    public void Draw_Sprite(SpriteBatch sb, Texture2D sprite, Rectangle image_rectangle, Vector2 pos, Vector2 orig, Vector2 image_scale, float rot, Color col)
    {
        sb.Draw(sprite, pos - Main.screenPosition, image_rectangle, col, rot, orig, image_scale, SpriteEffects.None, 0);
    }
    public void Draw_Sprite(SpriteBatch sb, Texture2D sprite, Rectangle image_rectangle, Vector2 pos, Vector2 orig, Vector2 image_scale, float rot, Color col, float alpha)
    {
        var tempcol = new Color(col.R, col.G, col.B, 1 * alpha);
        sb.Draw(sprite, pos - Main.screenPosition, image_rectangle, tempcol, rot, orig, image_scale, SpriteEffects.None, 0);
    }

    public override void Draw(SpriteBatch sb)
    {
        if (texture != null)
        {
            Draw_Sprite(sb, texture, frame, position, origin, scale, rotation, color);
        }
    }
}