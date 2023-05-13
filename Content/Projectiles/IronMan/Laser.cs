using MarvelTerrariaUniverse.AssetManager;
using MarvelTerrariaUniverse.Content.Effects;
using MarvelTerrariaUniverse.Core.Effects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Projectiles.IronMan;

public class Laser : ModProjectile
{
    public override string Texture => Assets.Textures.EmptyPixel;

    public float Distance
    {
        get => Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }

    public float MaxDistance
    {
        get => Projectile.ai[1];
        set => Projectile.ai[1] = value;
    }

    public override void SetDefaults()
    {
        Projectile.width = 10;
        Projectile.height = 10;
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.maxPenetrate = 1;
        Projectile.tileCollide = false;
        Projectile.hide = false;
        Projectile.timeLeft = 2;
    }

    float distanceToTarget = 1f;
    public override bool PreDraw(ref Color color)
    {
        ParticleEntity.Instantiate<LaserParticle>(e =>
        {
            e.position = Projectile.Center;
            e.scale.X *= Distance * distanceToTarget;
            e.scale.Y = Projectile.scale;
            e.rotation = Projectile.velocity.ToRotation();
        });

        return false;
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        Vector2 unit = Projectile.velocity;
        float point = 0f;

        return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
            Projectile.Center + unit * Distance, 5, ref point);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.immune[Projectile.owner] = 5;

        distanceToTarget = Main.player[Projectile.owner].Distance(target.Center) / Distance;
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        UpdatePlayer(player);
        SetLaserPosition(player);
        CastLights();
    }

    private void SetLaserPosition(Player player)
    {
        for (Distance = 25; Distance <= MaxDistance; Distance += 5f)
        {
            var start = player.Center + Projectile.velocity * Distance;
            if (!Collision.CanHit(player.Center, 1, 1, start, 1, 1))
            {
                Distance -= 5f;
                break;
            }
        }
    }

    private void UpdatePlayer(Player player)
    {
        if (Projectile.owner == Main.myPlayer)
        {
            Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.netUpdate = true;
        }

        player.heldProj = Projectile.whoAmI;

    }

    private void CastLights()
    {
        DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
        Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - 25), 26, DelegateMethods.CastLight);
    }

    public override bool ShouldUpdatePosition() => false;

    public override void CutTiles()
    {
        DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
        Vector2 unit = Projectile.velocity;
        Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
    }
}