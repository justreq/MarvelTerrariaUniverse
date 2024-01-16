using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MarvelTerrariaUniverse.Content.Projectiles.Arsenal
{
    public class Flares : ModProjectile
    {
        private int dustColor = DustID.PinkTorch; // dust type
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 120; // 2 seconds
            Projectile.penetrate = -1;
            Projectile.frame = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Confused, 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //reflect projectile velocity
            Projectile.velocity = Vector2.Reflect(oldVelocity, Projectile.velocity);
            return false;
        }
        public override void AI()
        {

            if (Projectile.timeLeft < 60) // occurs when projectile has 1 second left
            {
                Projectile.frame = 0;
                dustColor = DustID.Smoke; //here dust changes
            }
            //spawn pink dust randomly around the projectile
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustColor, 0f, 0f, 0, default(Color), 1f);
            Main.dust[dust].velocity *= 0.5f;
            Main.dust[dust].scale *= 1.3f;
            Main.dust[dust].noGravity = false;

        }
    }
}
