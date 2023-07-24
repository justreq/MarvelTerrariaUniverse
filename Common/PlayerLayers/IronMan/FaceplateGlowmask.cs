using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using MarvelTerrariaUniverse.Common.Players;
using Microsoft.Xna.Framework;

namespace MarvelTerrariaUniverse.Common.PlayerLayers.IronMan;
public class FaceplateGlowmask : PlayerDrawLayer
{
    public override bool IsHeadLayer => true;

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        Player drawPlayer = drawInfo.drawPlayer;
        IronManPlayer modPlayer = drawPlayer.GetModPlayer<IronManPlayer>();

        if (drawPlayer.dead || drawPlayer.invis || drawPlayer.head == -1) return false;

        return modPlayer.Mark > 1;
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player drawPlayer = drawInfo.drawPlayer;
        Asset<Texture2D> texture = ModContent.Request<Texture2D>($"{nameof(MarvelTerrariaUniverse)}/Assets/Glowmasks/Faceplate{(int)drawInfo.drawPlayer.GetModPlayer<IronManPlayer>().CurrentHelmetState}");

        Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.bodyFrame.Width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition;
        Vector2 headVect = drawInfo.headVect;

        DrawData drawData = new(texture.Value, drawPos.Floor() + headVect, drawPlayer.bodyFrame, drawPlayer.GetModPlayer<IronManPlayer>().EyeSlitColor, drawPlayer.headRotation, headVect, 1f, drawInfo.playerEffect, 0)
        {
            shader = drawInfo.cHead
        };
        drawInfo.DrawDataCache.Add(drawData);
    }
}