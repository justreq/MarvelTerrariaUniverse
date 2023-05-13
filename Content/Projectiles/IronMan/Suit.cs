using MarvelTerrariaUniverse.AssetManager;
using MarvelTerrariaUniverse.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Projectiles.IronMan;
public class Suit : ModProjectile
{
    public override string Texture => Assets.Textures.EmptyPixel;

    public override void SetDefaults()
    {
        Projectile.width = 26;
        Projectile.height = 40;
        Projectile.friendly = true;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        return false;
    }

    public override void AI()
    {
        var player = Main.player[Projectile.owner];
        var ironManPlayer = player.GetModPlayer<IronManPlayer>();

        Projectile.velocity.X = 0;
        Projectile.velocity.Y += 0.1f;
        if (Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f;

        Projectile.velocity.X = Projectile.velocity.X * 0.97f;
        Projectile.rotation += Projectile.velocity.X * 0.1f;

        if (player.GetModPlayer<BasePlayer>().transformation != Transformations.IronMan || Projectile.Hitbox.Contains(Main.MouseWorld.ToPoint()) && PlayerInput.Triggers.JustPressed.MouseLeft && player.position.WithinRange(Projectile.position, 4 * 16f))
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
            }

            Projectile.Kill();
            ironManPlayer.SuitEjected = false;
            ironManPlayer.SuitOn = true;
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var player = Main.player[Projectile.owner];
        var ironManPlayer = player.GetModPlayer<IronManPlayer>();

        Player drawPlayer = new()
        {
            direction = ironManPlayer.SuitDirection,
            head = EquipLoader.GetEquipSlot(Mod, $"IronManMark{ironManPlayer.Mark}", EquipType.Head),
            body = EquipLoader.GetEquipSlot(Mod, $"IronManMark{ironManPlayer.Mark}", EquipType.Body),
            legs = EquipLoader.GetEquipSlot(Mod, $"IronManMark{ironManPlayer.Mark}", EquipType.Legs)
        };

        Lighting.AddLight((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, TorchID.Torch, 0.5f);

        var position = Projectile.Center - Projectile.Size / 2;
        Main.PlayerRenderer.DrawPlayer(Main.Camera, drawPlayer, new Vector2(position.X + 4f, position.Y - 2f), Projectile.rotation, Projectile.Hitbox.Size() / 2f);

        return false;
    }

    public override void PostDraw(Color lightColor)
    {
        var texture = Assets.ToTexture2D(Assets.Textures.Glowmasks.IronMan.Faceplate0).Value;
        var position = Projectile.Center - Main.screenPosition - Projectile.Size / 2;
        Main.EntitySpriteDraw(texture, new Vector2(position.X + 7f, position.Y + 8f), new Rectangle(0, 0, texture.Width, texture.Height / 20), Color.White, Projectile.rotation, Projectile.Hitbox.Size() / 2f, 1f, Main.player[Projectile.owner].GetModPlayer<IronManPlayer>().SuitDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
    }
}