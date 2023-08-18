using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MarvelTerrariaUniverse.Content.Projectiles.Arsenal;
using MarvelTerrariaUniverse.Content.Buffs;

namespace MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
public class ArsenalFlares : ArsenalItem
{
    private bool activateCd = true;
    private bool launchCd = false;
    private int cdTimer = 0;
    private int launchDelay = 0;

    private int flareRotationTracker = 1;
    private float angleTest = MathHelper.PiOver4;
    public override void UpdateArsenal(Player player)
    {
        base.UpdateArsenal(player);
        if (player.HasBuff(ModContent.BuffType<Waterlogged>()))
        {
            return;
        }
        if (activateCd) { launchCd = true; activateCd = false; }
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        base.UpdateAccessory(player, hideVisual);

        cdTimer += 1;
        if (cdTimer >= 15 * 60)
        {
            activateCd = true;
            cdTimer = 0;
        }
        if (launchCd)
        {
            launchDelay += 1;

            if (launchDelay % 60 == flareRotationTracker) //flareRotationTracker is modified for frequency
            {
                angleTest = angleTest + Main.rand.NextFloat(-0.2f, 0.2f);
                Vector2 randomVelocity = angleTest.ToRotationVector2();
                //"Main.rand.Next(2,4)" can be set higher to change speed of projectiles. For example, just changing it to 20 would shoot really fast flares
                randomVelocity = Vector2.Normalize(randomVelocity) * Main.rand.Next(2, 4); 
                // dmg modified in below line
                Projectile.NewProjectile(Terraria.Entity.GetSource_None(), player.Center, randomVelocity, ModContent.ProjectileType<Flares>(), 10, 1, player.whoAmI);
                if (flareRotationTracker >= 49) { flareRotationTracker = 1; angleTest = MathHelper.PiOver4; }
                // in the else line, the 6 is the number of frames between each flare launch
                else { flareRotationTracker += 6; angleTest += MathHelper.PiOver4; }
            }
            if (launchDelay >= 180) //180 frames = 3 seconds of firing flares with above calculations
            {
                launchDelay = 0;
                launchCd = false;
            }
        }

    }
}
