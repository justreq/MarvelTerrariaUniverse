using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria;

namespace MarvelTerrariaUniverse.Content.Projectiles.Arsenal
{
    public class Repulsor : ModProjectile
    {
        private int frameCounter = 0;
        private enum ProjectileType
        {
            Repulsor,
            Unibeam
        }
        private bool unibeamCorrection = false;
        private enum Phase
        {
            Charging,
            Firing,
            End
        }

        private ProjectileType ProjType
        {
            get => (ProjectileType)Projectile.ai[0];
            set => Projectile.ai[0] = (int)value;
        }
        private Phase currentPhase = Phase.Charging;
        //The distance of beam from the player center
        private const float MOVE_DISTANCE = 25;
        public float Distance
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = false;
            Projectile.timeLeft = 55; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)

        }

        public override bool PreDraw(ref Color color)
        {
            // Needed because of parameters
            Texture2D Texture = ModContent.Request<Texture2D>($"MarvelTerrariaUniverse/Content/Projectiles/Arsenal/Repulsor").Value;
            DrawLaser(Texture, Main.player[Projectile.owner].Center, Projectile.velocity, 10, -1.57f, 1f, 1000f, Color.White, (int)MOVE_DISTANCE);

            return false;
        }

        // Current laser frame index
        private int laserAnimationFrame = 0;

        // The core function of drawing a laser
        private void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float step, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50)
        {
            float r = unit.ToRotation() + rotation;
            int currentLaserFrame = 0; // Alternates between 0 and 1
            int animationSpeed = 10; // Number of frames before switching to the next frame
            int startLoop = 5;
            int endLoop = 6;
            if (currentPhase == Phase.Charging)
            {
                currentLaserFrame = (laserAnimationFrame / animationSpeed % 2);
            }
            if (currentPhase == Phase.Firing)
            {
                currentLaserFrame = laserAnimationFrame / animationSpeed % (endLoop - startLoop + 1) + startLoop;
            }
            if (currentPhase == Phase.End)
            {
                currentLaserFrame = 7;
            }

            #region Laser Body
            // Draws the laser 'body'

            int bodyFrameWidth = 22;
            int bodyFrameHeight = 20;

            Rectangle bodySourceRect = new Rectangle(currentLaserFrame * bodyFrameWidth, 18, bodyFrameWidth, bodyFrameHeight);

            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                var origin = start + i * unit;
                Main.spriteBatch.Draw(texture, origin - Main.screenPosition,
                     bodySourceRect, i < transDist ? Color.Transparent : c, r,
                     new Vector2(bodyFrameWidth * .5f, bodyFrameHeight * .5f), scale, 0, 0);
            }
            #endregion
            #region Laser Tail
            // Draws the laser 'tail'

            int tailFrameWidth = 22; // Width of each animation frame
            int tailFrameHeight = 16; // Height of each animation frame

            Rectangle tailSourceRect = new Rectangle(currentLaserFrame * tailFrameWidth, 0, tailFrameWidth, tailFrameHeight);

            Main.spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                tailSourceRect, Color.White, r, new Vector2(tailFrameWidth * .5f, tailFrameHeight * .5f), scale, 0, 0);
            #endregion
            #region Laser Head
            // Draws the laser 'head'
            int headFrameWidth = 22;
            int headFrameHeight = 18;

            Rectangle headSourceRect = new Rectangle(currentLaserFrame * headFrameWidth, 40, headFrameWidth, headFrameHeight);

            Main.spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                headSourceRect, Color.White, r, new Vector2(headFrameWidth * .5f, headFrameHeight * .5f), scale, 0, 0);
            #endregion
            laserAnimationFrame++;
        }

        // Change the way of collision check of the Projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (currentPhase == Phase.Charging)
                return false;
            // Initial check
            if (projHitbox.Intersects(targetHitbox))
            {
                SetLaserPosition(targetHitbox);
                return true;
            }
            float _ = 0f;
            // Deeper check
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Distance, Projectile.Size.Length() * Projectile.scale, ref _))
            {
                SetLaserPosition(targetHitbox);
                return true;
            }
            return false;
        }


        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ProjType == ProjectileType.Repulsor)
            {
                Projectile.damage = 0;
            }
        }

        // The AI of the Projectile
        public override void AI()
        {
            frameCounter++;

            if (ProjType == ProjectileType.Repulsor)
            {
                if (frameCounter >= 20 && frameCounter <= 50)
                {
                    currentPhase = Phase.Firing;
                }
                if (frameCounter >= 50)
                {
                    currentPhase = Phase.End;
                }
            }

            if (ProjType == ProjectileType.Unibeam)
            {
                if (!unibeamCorrection)
                {
                    Projectile.timeLeft = 180;
                    unibeamCorrection = true;
                }
                if (frameCounter >= 20 && frameCounter <= 170)
                {
                    currentPhase = Phase.Firing;
                }
                if (frameCounter >= 170)
                {
                    currentPhase = Phase.End;
                }
            }
            
            Player player = Main.player[Projectile.owner];
            Projectile.position = player.Center + Projectile.velocity * MOVE_DISTANCE;
            UpdatePlayer(player);
            if (currentPhase != Phase.Charging)
            {
            SetLaserPosition(new Rectangle());
                //SpawnDusts(player);
                CastLights();
            }
        }

        private void SpawnDusts(Player player)
        {
            Vector2 unit = Projectile.velocity * -1;
            Vector2 dustPos = player.Center + Projectile.velocity * Distance;

            for (int i = 0; i < 2; ++i)
            {
                // Generate random rotation angle
                float num1 = Projectile.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;

                // Generate random scale and velocity for the dust
                float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);

                // Create dust particle at the specified position with velocity
                Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, 31, dustVel.X, dustVel.Y)];
                dust.noGravity = true;
                dust.scale = 1.2f;

                //// Create another dust particle at a different position with velocity
                //dust = Dust.NewDustDirect(Main.player[Projectile.owner].Center, 0, 0, 31,
                //    -unit.X * Distance, -unit.Y * Distance);
                //dust.fadeIn = 0f;
                //dust.noGravity = true;
                //dust.scale = 0.88f;
                //dust.color = Color.Yellow;
            }

            // Generate additional dust effect with a chance
            if (Main.rand.NextBool(5))
            {
                // Generate random offset based on projectile velocity
                Vector2 offset = Projectile.velocity.RotatedBy(1.57f) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;

                // Create dust particle at the specified offset position
                Dust dust = Main.dust[Dust.NewDust(dustPos + offset - Vector2.One * 4f, 8, 8, 31, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity *= 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);

                // Create another dust particle at a different position with velocity
                unit = dustPos - Main.player[Projectile.owner].Center;
                unit.Normalize();
                dust = Main.dust[Dust.NewDust(Main.player[Projectile.owner].Center + 55 * unit, 8, 8, 31, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity = dust.velocity * 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                dust.color = Color.Yellow;
            }
        }


        /*
        * Sets the end of the laser position based on where it collides with something
        */
        public float DetermineLaserLength_CollideWithTiles(int samplePointCount)
        {
            float[] laserLengthSamplePoints = new float[samplePointCount];
            Collision.LaserScan(Projectile.Center, Projectile.velocity, Projectile.scale, 1000f, laserLengthSamplePoints);
            return laserLengthSamplePoints.Average();
        }
        private void SetLaserPosition(Rectangle TargetHitbox)
        {
            float laserLength = DetermineLaserLength_CollideWithTiles(100); // You can adjust the sample point count
            if (TargetHitbox == new Rectangle())
            {
                Distance = laserLength + 10;
            }
            else
            {
                Distance = TargetHitbox.Distance(Projectile.Center) + 10;
            }
        }



        private void UpdatePlayer(Player player)
        {
            // Multiplayer support here, only run this code if the client running it is the owner of the Projectile
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 diff = Main.MouseWorld - player.Center;
                diff.Normalize();
                Projectile.velocity = diff;
                Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                Projectile.netUpdate = true;
            }
            int dir = Projectile.direction;
            player.ChangeDir(dir); // Set player direction to where we are shooting
            player.heldProj = Projectile.whoAmI; // Update player's held Projectile

        }

        private void CastLights()
        {
            // Cast a light along the line of the laser
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - MOVE_DISTANCE), 26, DelegateMethods.CastLight);
        }

        public override bool ShouldUpdatePosition() => false;

        /*
        * Update CutTiles so the laser will cut tiles (like grass)
        */
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackMelee;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * 1000f, Projectile.Size.Length() * Projectile.scale, DelegateMethods.CutTiles);
        }
    }
}
