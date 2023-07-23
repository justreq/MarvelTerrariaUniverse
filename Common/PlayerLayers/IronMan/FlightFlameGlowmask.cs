using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using MarvelTerrariaUniverse.Common.Players;
using Microsoft.Xna.Framework;

namespace MarvelTerrariaUniverse.Common.PlayerLayers.IronMan;
public class FlightFlameGlowmask : PlayerDrawLayer
{
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        Player drawPlayer = drawInfo.drawPlayer;
        IronManPlayer modPlayer = drawPlayer.GetModPlayer<IronManPlayer>();

        if (drawPlayer.dead || drawPlayer.invis || drawPlayer.head == -1) return false;

        return modPlayer.Mark > 1 && modPlayer.CurrentSuitState != IronManPlayer.SuitState.None;
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player drawPlayer = drawInfo.drawPlayer;
        IronManPlayer modPlayer = drawPlayer.GetModPlayer<IronManPlayer>();
        Asset<Texture2D> texture = ModContent.Request<Texture2D>($"{nameof(MarvelTerrariaUniverse)}/Assets/Glowmasks/FlightFlame{(modPlayer.CurrentSuitState == IronManPlayer.SuitState.Hovering ? "Alt" : "")}");

        int frameCount = modPlayer.CurrentSuitState == IronManPlayer.SuitState.Hovering ? 2 : 3;
        Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - 10f - (drawPlayer.direction == 1 ? 2f + (modPlayer.CurrentSuitState == IronManPlayer.SuitState.Flying ? 2f : 0f) : 4f), drawPlayer.height - 8f) + drawPlayer.legPosition;
        Vector2 legsOffset = drawInfo.legsOffset;
        Rectangle frame = new(0, texture.Height() / frameCount * (modPlayer.FlightFlameFrameCounter % frameCount), texture.Width(), texture.Height() / frameCount);

        DrawData drawData = new(texture.Value, drawPos.Floor() + legsOffset, frame, Color.White, drawPlayer.legRotation, legsOffset, 1f, drawInfo.playerEffect, 0);
        drawInfo.DrawDataCache.Add(drawData);
    }
}