using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Projectiles.Arsenal
{
    public class CrudeMissile : ModProjectile
    {
        private int explosionRadius = 100;
        public override void SetDefaults()
        {
            Projectile.Name = "Crude Missile";
            Projectile.width = 26;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
        }

        private void Explode()
        {
            Projectile.Resize(explosionRadius, explosionRadius);
            Projectile.alpha = 255;
            if (Projectile.ai[0] == 0)
            {
                Projectile.Kill();
            }
            else if (Projectile.ai[0] == 1)
            {
                Projectile.timeLeft = 3;
                Projectile.ai[0] = 2;
            }
        }
        public override void AI()
        {
            if (Projectile.ai[0] != 0)
            {
                Explode();
            }
            else
            {
                //leave dust trail
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                    dust.velocity *= 0.1f;
                    dust.noGravity = true;
                }
                Projectile.rotation = Projectile.velocity.ToRotation()/* + MathHelper.PiOver2*/;
                if (Projectile.timeLeft <= 5)
                {
                    Explode();
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 0)
            {
                Explode();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] != 0)
            {
                return false;
            }
            Explode();
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer && Projectile.ai[0] == 0)
            {  
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Projectile.type, Projectile.damage, Projectile.knockBack, Main.myPlayer, 1);
                //Sound
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                for (int i = 0; i < 50; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                    dust.velocity *= 1.4f;
                }

                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                    dust.noGravity = true;
                    dust.velocity *= 5f;
                    dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                    dust.velocity *= 3f;
                }

                // Large Smoke Gore spawn
                for (int g = 0; g < 2; g++)
                {
                    var goreSpawnPosition = new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f);
                    Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                    gore.scale = 1.5f;
                    gore.velocity.X += 1.5f;
                    gore.velocity.Y += 1.5f;
                    gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                    gore.scale = 1.5f;
                    gore.velocity.X -= 1.5f;
                    gore.velocity.Y += 1.5f;
                    gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                    gore.scale = 1.5f;
                    gore.velocity.X += 1.5f;
                    gore.velocity.Y -= 1.5f;
                    gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                    gore.scale = 1.5f;
                    gore.velocity.X -= 1.5f;
                    gore.velocity.Y -= 1.5f;
                }
            }
        }
    }
}
