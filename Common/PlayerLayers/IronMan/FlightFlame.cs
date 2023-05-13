using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.AssetManager;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Common.PlayerLayers.IronMan;
public sealed class FlightFlame : PlayerDrawLayer
{
    public override Position GetDefaultPosition()
    {
        return new BeforeParent(PlayerDrawLayers.Leggings);
    }

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        var basePlayer = drawInfo.drawPlayer.GetModPlayer<BasePlayer>();
        var ironManPlayer = drawInfo.drawPlayer.GetModPlayer<IronManPlayer>();

        return basePlayer.transformation == Transformations.IronMan && ironManPlayer.Mark > 1 && ironManPlayer.Flying;
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var drawPlayer = drawInfo.drawPlayer;
        var ironManPlayer = drawPlayer.GetModPlayer<IronManPlayer>();

        var texture = Assets.ToTexture2D(ironManPlayer.Hovering ? Assets.Textures.Glowmasks.IronMan.FlightFlameAlt : Assets.Textures.Glowmasks.IronMan.FlightFlame).Value;
        var drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - 10f - (drawPlayer.direction == 1 ? 2f + (!ironManPlayer.Hovering ? 2f : 0f) : 4f), drawPlayer.height - 8f) + drawPlayer.legPosition;
        var legsOffset = drawInfo.legsOffset;

        DrawData drawData = new(texture, drawPos.Floor() + legsOffset, new Rectangle(0, ironManPlayer.FlightFlameFrame * (texture.Height / (ironManPlayer.Hovering ? 2 : 3)), texture.Width, texture.Height / (ironManPlayer.Hovering ? 2 : 3)), Color.White, drawPlayer.legRotation, legsOffset, 1f, drawInfo.playerEffect, 0);
        drawInfo.DrawDataCache.Add(drawData);
    }
}
