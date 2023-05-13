using Terraria;
using MarvelTerrariaUniverse.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using MarvelTerrariaUniverse.AssetManager;

namespace MarvelTerrariaUniverse.Content.Projectiles.IronMan;
public class Helmet : ModProjectile
{
    public override string Texture => Assets.Textures.EmptyPixel;

    public override void SetDefaults()
    {
        Projectile.width = 18;
        Projectile.height = 20;
        Projectile.friendly = true;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f) Projectile.velocity.X = oldVelocity.X * -0.3f;
        if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f) Projectile.velocity.Y = oldVelocity.Y * -0.3f;

        return false;
    }

    public override void AI()
    {
        var player = Main.player[Projectile.owner];
        var ironManPlayer = player.GetModPlayer<IronManPlayer>();

        Projectile.velocity.Y += 0.1f;
        if (Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f;

        if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
        {
            if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
            {
                Projectile.velocity.X = 0f;
                Projectile.netUpdate = true;
            }
        }

        Projectile.velocity.X = Projectile.velocity.X * 0.97f;
        Projectile.rotation += Projectile.velocity.X * 0.1f;

        if (player.GetModPlayer<BasePlayer>().transformation != Transformations.IronMan || Projectile.Hitbox.Contains(Main.MouseWorld.ToPoint()) && PlayerInput.Triggers.JustPressed.MouseLeft && player.position.WithinRange(Projectile.position, 4 * 16f))
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
            }

            Projectile.Kill();
            ironManPlayer.HelmetDropped = false;
            ironManPlayer.HelmetOn = true;
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var player = Main.player[Projectile.owner];
        var ironManPlayer = player.GetModPlayer<IronManPlayer>();

        var Texture = Assets.ToTexture2D($"{Assets.Textures.Path}/Projectiles/IronMan/Helmet{ironManPlayer.Mark}").Value;
        var HelmetFrame = ironManPlayer.FaceplateOn ? 0 : 1;

        Main.spriteBatch.Draw(Texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Texture.Height / 2 * HelmetFrame, Texture.Width, Texture.Height / 2), Color.White, Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0);

        return false;
    }
}