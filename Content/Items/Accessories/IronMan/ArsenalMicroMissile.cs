using MarvelTerrariaUniverse.Content.Buffs;
using MarvelTerrariaUniverse.Content.Projectiles.Arsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
public class ArsenalMicroMissile : ArsenalItem
{
    int cd = 0;
    bool offCD = true;
    public override void UpdateArsenal(Player player)
    {
        base.UpdateArsenal(player);
        if (player.HasBuff(ModContent.BuffType<Waterlogged>()))
        {
            return;
        }
        if (player.HasBuff(ModContent.BuffType<Waterlogged>()))
        {
            return;
        }
        Vector2 mousePos = Main.MouseWorld;
        Vector2 relativeMousePos = mousePos - player.Center;
        relativeMousePos = Vector2.Normalize(relativeMousePos) * 10;
        if (offCD)
        {
            //adjust dmg in the projectile spawn here to balance
            Projectile.NewProjectile(Terraria.Entity.GetSource_None(), player.Center, relativeMousePos, ModContent.ProjectileType<Missile>(), 50, 10);
            offCD = false;
        }
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);
        if (!offCD)
        {
            cd++;
            if (cd > 300)
            {
                offCD = true;
                cd = 0;
            }
        }
    }
}
