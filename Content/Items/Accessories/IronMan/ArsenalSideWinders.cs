using MarvelTerrariaUniverse.Content.Projectiles.Arsenal;
using Microsoft.Xna.Framework;
using System.Threading;
using Terraria;
using Terraria.ModLoader;
using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.Content.Buffs;

namespace MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
public class ArsenalSideWinders : ArsenalItem
{
    //get the current instance of iron man player
    private int ArmorMode = 0;
    private int cd = 0;
    private int releaseTimer = 0;
    private bool offCD = true;
    private bool activated = false;
    public override void UpdateArsenal(Player player)
    {
        base.UpdateArsenal(player);
        if (player.HasBuff(ModContent.BuffType<Waterlogged>()))
        {
            return;
        }
        if (offCD) activated = true;

    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);
        IronManPlayer ironManPlayer = Main.LocalPlayer.GetModPlayer<IronManPlayer>();
        ArmorMode = ironManPlayer.ArmorMode;
        // create a vector 2 that just goes down
        if (ArmorMode == 0)
        {
            Vector2 down = new Vector2(0, 1);
            down = Vector2.Normalize(down) * 10;
            if (activated)
            {
                releaseTimer++;
                if (releaseTimer % 10 == 0)
                {
                    //adjust dmg in the projectile spawn here to balance
                    Projectile.NewProjectile(Terraria.Entity.GetSource_None(), player.Center, down, ModContent.ProjectileType<Missile>(), 50, 10);
                }
                if (releaseTimer > 80)
                {
                    activated = false;
                    offCD = false;
                    releaseTimer = 0;
                }
            }
            if (!offCD)
            {
                cd++;
                if (cd > 20 * 60)
                {
                    offCD = true;
                    cd = 0;
                }
            }
        }
    }
}
