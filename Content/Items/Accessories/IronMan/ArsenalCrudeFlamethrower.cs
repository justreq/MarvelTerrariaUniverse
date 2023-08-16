using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
public class ArsenalCrudeFlamethrower : ArsenalItem
{
    private int cooldown = 0;
    private int ammoFrameLimit = 0;
    public override void UpdateArsenal(Player player)
    {
        // this runs every frame when you have the arsenal selected and press the left mouse button

        //limiting ammo usage so its not as fast as my old car uses gas
        //also making it pause every few frames so its not OP - WIP
        

        //calculate the vector 2 of the mouse position relative to the player
        Vector2 mousePos = Main.MouseWorld;
        Vector2 relativeMousePos = mousePos - player.Center;
        //draw a circle around the center of the player and if the relative mouse pos goes outside the circle make it the closest point on the circle
        if (relativeMousePos.Length() > 7)
        {
            relativeMousePos = Vector2.Normalize(relativeMousePos) * 7;
        }
        //player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, angle);
        //check if the player has gel in their inventory
        if (player.CountItem(ItemID.Gel) > 0)
        {
            //2nd to last number is dmg value, needs balancing
            Projectile.NewProjectile(Terraria.Entity.GetSource_None(), player.Center, relativeMousePos, ProjectileID.Flames, 5, 1);
            
            // Cant figure out arm rotation
            //player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.Center - Main.MouseWorld).ToRotation() + MathHelper.PiOver2 - player.fullRotation);
            
            
            ammoFrameLimit++;
            if (ammoFrameLimit > 15) //2 gel a second
            {
               player.ConsumeItem(ItemID.Gel);
               ammoFrameLimit = 0;
            }
        }

    }
}
