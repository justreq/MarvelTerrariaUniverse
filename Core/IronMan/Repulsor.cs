using MarvelTerrariaUniverse.AssetManager;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria;
using MarvelTerrariaUniverse.Content.Projectiles.IronMan;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Core.IronMan;
public class Repulsor : Ability
{
    public Player Player => Main.LocalPlayer;
    public int cooldown;

    public Repulsor(Vector2 position, Vector2 velocity, int damage, float knockback, int cooldown) : base(position, velocity, damage, knockback)
    {
        this.cooldown = cooldown;
    }

    public void SpawnLaser(Vector2 position, Vector2 velocity, float scale)
    {
        var proj = Projectile.NewProjectile(Entity.GetSource_None(), position, velocity, ModContent.ProjectileType<Laser>(), 4, 4, Player.whoAmI, 0, 1200);
        Main.projectile[proj].scale = scale;
    }

    public void SpawnRepulsor()
    {
        float angle = Player.AngleTo(Main.MouseWorld) - MathHelper.PiOver2 - Player.fullRotation;
        Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, angle);

        cooldown--;

        if (cooldown <= 0)
        {
            if (cooldown == 0) SoundEngine.PlaySound(Assets.ToSoundStyle(Assets.Sounds.IronMan.LaserBlast));
            SpawnLaser(Player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, angle), Player.DirectionTo(Main.MouseWorld), 0.3f);
        }
    }

    public void SpawnUnibeam()
    {
        cooldown--;

        if (cooldown <= 0)
        {
            if (cooldown == 0) SoundEngine.PlaySound(Assets.ToSoundStyle(Assets.Sounds.IronMan.LaserBlast));
            SpawnLaser(new(Player.Center.X + 4f * Player.direction, Player.Center.Y + 2f), Player.fullRotation.ToRotationVector2() * Player.direction, 0.5f);
        }
    }
}
