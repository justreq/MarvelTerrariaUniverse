using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.CodeAnalysis;

namespace MarvelTerrariaUniverse.Content.Items.Weapons.Mjolnir
{
    public class MjolnirProjectile : ModProjectile
    {
        private enum ChargeState
        {
            NotCharged,
            Charging,
            Charged
        }
        private enum AIStates
        {
            Throwing,
            Flying,
            Returning,
            Lightning
        } 

        public ref float Charge => ref Projectile.ai[1];
        private AIStates CurrentAIState
        {
            get => (AIStates)Projectile.ai[2];
            set => Projectile.ai[2] = (float)value;
        }

        #region Lightning fields
        private readonly HashSet<int> hitTargets = new HashSet<int>();

        private readonly HashSet<int> foundTargets = new HashSet<int>();

        private UnifiedRandom rng;

        public UnifiedRandom Rng
        {
            get
            {
                if (rng == null)
                {
                    rng = new UnifiedRandom(RandomSeed / (1 + base.Projectile.identity));
                }
                return rng;
            }
        }

        public int RandomSeed
        {
            get
            {
                return (int)base.Projectile.ai[0];
            }
            set
            {
                base.Projectile.ai[0] = value;
            }
        }

        public int WobbleTimer
        {
            get
            {
                return (int)base.Projectile.ai[1];
            }
            set
            {
                base.Projectile.ai[1] = value;
            }
        }

        public bool Spawned
        {
            get
            {
                return base.Projectile.localAI[0] == 1f;
            }
            set
            {
                base.Projectile.localAI[0] = (value ? 1f : 0f);
            }
        }
        #endregion

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
            if (CurrentAIState == AIStates.Lightning)
            {
                ProjectileID.Sets.CanDistortWater[base.Projectile.type] = false;
            }
        }

        public override void SetDefaults()
        {
            if (CurrentAIState != AIStates.Lightning)
            {
                Projectile.friendly = true;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 600;
                Projectile.frameCounter = 0;
                Projectile.frame = 6;
                Projectile.width = 44;
                Projectile.height = 46;
                Projectile.light = 2f;
            }
            else
            {
                #region Modifying projectile properties
                base.Projectile.width = 10;
                base.Projectile.height = 10;
                base.Projectile.aiStyle = -1;
                base.Projectile.penetrate = 1;
                base.Projectile.alpha = 255;
                base.Projectile.friendly = true;
                base.Projectile.DamageType = DamageClass.Ranged;
                base.Projectile.timeLeft = 600;
                base.Projectile.extraUpdates = 200;
                base.Projectile.ignoreWater = true;
                base.Projectile.usesLocalNPCImmunity = true;
                base.Projectile.localNPCHitCooldown = 10;

                //CatPlayer.ScreenShake(base.Projectile.position, 0.3f);
                base.Projectile.width = 10;
                base.Projectile.height = 10;
                base.Projectile.position.X -= 50f;
                base.Projectile.position.Y -= 50f;
                base.Projectile.Damage();
                #endregion
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CurrentAIState == AIStates.Throwing)
            {
                CurrentAIState = AIStates.Returning;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (CurrentAIState == AIStates.Lightning)
            {

                if (hitTargets.Contains(target.whoAmI))
                {
                    return false;
                }
            }
            return base.CanHitNPC(target);
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (CurrentAIState == AIStates.Throwing)
            {
                Projectile.velocity = oldVelocity * -0.75f;
                return false;
            }
            else if (CurrentAIState == AIStates.Lightning)
            {
                if ((double)base.Projectile.velocity.Y != (double)oldVelocity.Y || (double)base.Projectile.velocity.X != (double)oldVelocity.X)
                {
                    if ((double)base.Projectile.velocity.X != (double)oldVelocity.X)
                    {
                        base.Projectile.velocity.X = 0f - oldVelocity.X;
                    }
                    if ((double)base.Projectile.velocity.Y != (double)oldVelocity.Y)
                    {
                        base.Projectile.velocity.Y = 0f - oldVelocity.Y;
                    }
                }
                return false;
            }
            return true;
        }

        public override void AI()
        {
            #region Charge animation
            if (Charge == (float)ChargeState.NotCharged)
            {
                Projectile.frame = 6;
            }
            else if (Charge == (float)ChargeState.Charging)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 5)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 6)
                {
                    Projectile.frame = 0;
                }
            }
            else if (Charge == (float)ChargeState.Charged)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 2)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 6)
                {
                    Projectile.frame = 0;
                }
            }
            #endregion

            switch (CurrentAIState)
            {
                case AIStates.Throwing:
                    {
                        Projectile.rotation += 0.3f * Projectile.direction;
                        Projectile.velocity *= 0.99f;
                        if (Projectile.velocity.Length() < 1f)
                        {
                            CurrentAIState = AIStates.Returning;
                        }
                        break;
                    }
                case AIStates.Returning:
                    {
                        Projectile.velocity *= 1.05f;
                        Projectile.tileCollide = false;
                        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.1f);
                        Projectile.velocity += Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center) * 0.5f;
                        //if the projectile is within 1 tile of the player it should kill the projectile
                        if (Vector2.Distance(Main.player[Projectile.owner].Center, Projectile.Center) < 16f)
                        {
                            Projectile.Kill();
                        }
                        break;
                    }
                case AIStates.Lightning:
                    {
                        

                        Player player = Main.player[base.Projectile.owner];
                        int num = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 111, 0f, 0f, 75, default(Color), 1.35f);
                        Main.dust[num].position.X = base.Projectile.Center.X;
                        Main.dust[num].position.Y = base.Projectile.Center.Y;
                        Main.dust[num].velocity *= 0f;
                        Main.dust[num].scale = 0.6f;
                        Main.dust[num].noGravity = true;
                        if (!Spawned)
                        {
                            Spawned = true;
                            for (int i = 0; i < 3; i++)
                            {
                                int num2 = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 111, base.Projectile.velocity.X * 1f, base.Projectile.velocity.Y * 0.7f, 125, default(Color), 1.25f);
                                Main.dust[num2].noGravity = true;
                                Main.dust[num2].scale = 0.7f;
                            }
                        }
                        WobbleTimer++;
                        if (WobbleTimer > 2)
                        {
                            base.Projectile.velocity.Y += Rng.NextFloat(-0.75f, 0.75f);
                            base.Projectile.velocity.X += Rng.NextFloat(-0.75f, 0.75f);
                            WobbleTimer = 0;
                        }
                        float num3 = player.Center.X;
                        float num4 = player.Center.Y;
                        float num5 = 2000f;
                        bool flag = false;
                        float x = 0;
                        float y = 0;
                        if (Charge == (float)ChargeState.Charging)
                        {
                            for (int j = 0; j < 200; j++)
                            {

                                x = player.Center.X; //change for target of lightning
                                x = player.Center.Y; //^
                                float num6 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - x) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - y);
                                if (num6 < num5)
                                {
                                    num5 = num6;
                                    num3 = x;
                                    num4 = y;
                                    flag = true;
                                }

                            }
                        }
                        else if (Charge == (float)ChargeState.Charged)
                        {
                            for (int j = 0; j < 200; j++)
                            {
                                NPC nPC = Main.npc[j];
                                if (nPC.CanBeChasedBy() && !hitTargets.Contains(nPC.whoAmI) && base.Projectile.DistanceSQ(nPC.Center) < num5 * num5 && Collision.CanHit(base.Projectile.Center, 1, 1, nPC.Center, 1, 1))
                                {
                                    x = nPC.Center.X;
                                    y = nPC.Center.Y;
                                }

                                if (x == 0 && y == 0)
                                {
                                    x = Main.MouseWorld.X;
                                    y = Main.MouseWorld.Y;
                                }

                                float num6 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - x) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - y);
                                if (num6 < num5)
                                {
                                    num5 = num6;
                                    num3 = x;
                                    num4 = y;
                                    flag = true;
                                }
                            }
                        }
                        if (flag)
                        {
                            float num7 = 2.5f;
                            Vector2 center = base.Projectile.Center;
                            float num8 = num3 - center.X;
                            float num9 = num4 - center.Y;
                            float num10 = (float)Math.Sqrt(num8 * num8 + num9 * num9);
                            if (num10 > 0f)
                            {
                                num10 = num7 / num10;
                            }
                            num8 *= num10;
                            num9 *= num10;
                            base.Projectile.velocity.X = (base.Projectile.velocity.X * 20f + num8) / 21f;
                            base.Projectile.velocity.Y = (base.Projectile.velocity.Y * 20f + num9) / 21f;
                        }
                        else
                        {
                            base.Projectile.velocity = Vector2.Zero;
                        }

                        break;
                    }
            }

        }
    }
}
