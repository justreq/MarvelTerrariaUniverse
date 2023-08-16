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
    public class MjolnirThrow : ModProjectile
    {
        int mode = 0;

         public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            //Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            //Projectile.direction = 1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            mode = 1;
        }
        public override void AI()
        {
            if (mode == 0)
            {
                Projectile.rotation += 0.3f * Projectile.direction;
                Projectile.velocity *= 0.99f;
                if (Projectile.velocity.Length() < 1f)
                {
                    mode = 1;
                }
            }

            //with mode 1 the projectile should face the player and fly towards them
            if (mode == 1)
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
            }
        }
        //make the projectile bounce upon tile collision
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity * -0.75f;
            return false;
        }
    }
    public class MjolnirChargedThrow : ModProjectile
    {
        int mode = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            //Projectile.direction = 1;
            Projectile.frameCounter = 0;
            Projectile.frame = 0;
            Projectile.width = 44;
            Projectile.height = 46;
            Projectile.light = 2f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            mode = 1;
        }
        public override void AI()
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

            if (mode == 0)
            {
                Projectile.rotation += 0.3f * Projectile.direction;
                Projectile.velocity *= 0.99f;
                if (Projectile.velocity.Length() < 1f)
                {
                    mode = 1;
                }
            }

            //with mode 1 the projectile should face the player and fly towards them
            if (mode == 1)
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
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity * -0.75f;
            return false;
        }
    }
   
    public class MjolnirFly : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.direction = 1;
        }
        public override void AI()
        {
            //Player player = Main.player[Projectile.owner];
            //Projectile.Center = player.Center;
            //Projectile.rotation = Projectile.velocity.ToRotation();
            Player player = Main.player[Projectile.owner];
            Vector2 playerPos = player.Center;

            // Get the mouse position in the world
            Vector2 mousePos = Main.MouseWorld;

            // Calculate the direction vector towards the mouse
            Vector2 direction = mousePos - playerPos;
            direction.Normalize();

            // Set the projectile's velocity to move the player
            float speed = 10f; // Adjust this to control the player's movement speed
            player.velocity = direction * speed;

            // Set the player's direction based on the movement
            if (player.velocity.X > 0f)
            {
                player.direction = 1;
            }
            else if (player.velocity.X < 0f)
            {
                player.direction = -1;
            }

            // Sync the player's position with the client and server
            player.position += player.velocity;
        }
    }

    #region MjolnirLightning
    public class MjolnirLightning : ModProjectile
    {
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

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CanDistortWater[base.Projectile.type] = false;
        }

        public override void SetDefaults()
        {
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
        }
        //public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        //{
        //    //Projectile.NewProjectile(base.Projectile.Center.X + base.Projectile.velocity.X, base.Projectile.Center.Y + base.Projectile.velocity.Y, base.Projectile.velocity.X * 0, base.Projectile.velocity.Y * 0, base.mod.ProjectileType("Flash"), base.Projectile.damage * 0, base.Projectile.knockBack * 0.85f, base.Projectile.owner);
        //}
        public override bool? CanHitNPC(NPC target)
        {
            if (hitTargets.Contains(target.whoAmI))
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void AI()
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
            float num5 = 5000f;
            bool flag = false;
            float x = 0;
            float y = 0;
            for (int j = 0; j < 200; j++)
            {
                NPC nPC = Main.npc[j];
                if (nPC.CanBeChasedBy() && !hitTargets.Contains(nPC.whoAmI) && base.Projectile.DistanceSQ(nPC.Center) < num5 * num5 && Collision.CanHit(base.Projectile.Center, 1, 1, nPC.Center, 1, 1))
                {
                    x = nPC.Center.X;
                    y = nPC.Center.Y;
                }

                if(x == 0 && y == 0)
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
        }
        public override bool OnTileCollide(Vector2 velocity1)
        {
            if ((double)base.Projectile.velocity.Y != (double)velocity1.Y || (double)base.Projectile.velocity.X != (double)velocity1.X)
            {
                if ((double)base.Projectile.velocity.X != (double)velocity1.X)
                {
                    base.Projectile.velocity.X = 0f - velocity1.X;
                }
                if ((double)base.Projectile.velocity.Y != (double)velocity1.Y)
                {
                    base.Projectile.velocity.Y = 0f - velocity1.Y;
                }
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                int num = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 255, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 75, default(Color), 0.75f);
                Main.dust[num].noGravity = true;
                Main.dust[num].scale = 1.5f;
                for (int f = 0; f < 2; f++)
                {
                    int num2 = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 255, base.Projectile.velocity.X * 1f, base.Projectile.velocity.Y * 0f, 125, default(Color), 1.25f);
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].scale = 1.5f;
                    {
                        for (int k = 0; k < 0.3; k++)
                        {
                            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0.0f, -1f, 0, new Color(), 1f);
                            Main.dust[dust].noGravity = false;
                            Dust dust1 = Main.dust[dust];
                            dust1.position.X = dust1.position.X + ((float)(Main.rand.Next(-0, 0) / 1) - 1f);
                            Dust dust2 = Main.dust[dust];
                            dust2.position.Y = dust2.position.Y + ((float)(Main.rand.Next(-0, 0) / 1) - 1f);
                            if (Main.dust[dust].position != Projectile.Center) Main.dust[dust].velocity = Projectile.DirectionTo(Main.dust[dust].position) * 2.0f;

                        }
                    }
                }
            }
        }
    }


    public class MjolnirCharge : ModProjectile
    {
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

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CanDistortWater[base.Projectile.type] = false;
        }

        public override void SetDefaults()
        {
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
        }
        //public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        //{
        //    //Projectile.NewProjectile(base.Projectile.Center.X + base.Projectile.velocity.X, base.Projectile.Center.Y + base.Projectile.velocity.Y, base.Projectile.velocity.X * 0, base.Projectile.velocity.Y * 0, base.mod.ProjectileType("Flash"), base.Projectile.damage * 0, base.Projectile.knockBack * 0.85f, base.Projectile.owner);
        //}
        public override bool? CanHitNPC(NPC target)
        {
            if (hitTargets.Contains(target.whoAmI))
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void AI()
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
            for (int j = 0; j < 200; j++)
            {

                float x = player.Center.X; //change for target of lightning
                float y = player.Center.Y; //^
                float num6 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - x) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - y);
                if (num6 < num5)
                {
                    num5 = num6;
                    num3 = x;
                    num4 = y;
                    flag = true;
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
        }
        public override bool OnTileCollide(Vector2 velocity1)
        {
            if ((double)base.Projectile.velocity.Y != (double)velocity1.Y || (double)base.Projectile.velocity.X != (double)velocity1.X)
            {
                if ((double)base.Projectile.velocity.X != (double)velocity1.X)
                {
                    base.Projectile.velocity.X = 0f - velocity1.X;
                }
                if ((double)base.Projectile.velocity.Y != (double)velocity1.Y)
                {
                    base.Projectile.velocity.Y = 0f - velocity1.Y;
                }
            }
            return false;
        }

        //public override void Kill(int timeLeft)
        //{
        //    for (int i = 0; i < 2; i++)
        //    {
        //        int num = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 255, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 75, default(Color), 0.75f);
        //        Main.dust[num].noGravity = true;
        //        Main.dust[num].scale = 1.5f;
        //        for (int f = 0; f < 2; f++)
        //        {
        //            int num2 = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 255, base.Projectile.velocity.X * 1f, base.Projectile.velocity.Y * 0f, 125, default(Color), 1.25f);
        //            Main.dust[num2].noGravity = true;
        //            Main.dust[num2].scale = 1.5f;
        //            {
        //                for (int k = 0; k < 0.3; k++)
        //                {
        //                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0.0f, -1f, 0, new Color(), 1f);
        //                    Main.dust[dust].noGravity = false;
        //                    Dust dust1 = Main.dust[dust];
        //                    dust1.position.X = dust1.position.X + ((float)(Main.rand.Next(-0, 0) / 1) - 1f);
        //                    Dust dust2 = Main.dust[dust];
        //                    dust2.position.Y = dust2.position.Y + ((float)(Main.rand.Next(-0, 0) / 1) - 1f);
        //                    if (Main.dust[dust].position != Projectile.Center) Main.dust[dust].velocity = Projectile.DirectionTo(Main.dust[dust].position) * 2.0f;

        //                }
        //            }
        //        }
        //    }
    }
    #endregion
}


