using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.Common.UIElements;
using MarvelTerrariaUniverse.Common.UIElements.SuitModuleHubUI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Common.PlayerLayers.IronMan;
public class HelmetGlowmask : PlayerDrawLayer
{
    private static Dictionary<int, DrawLayerData> HeadLayerData { get; set; }

    public static void RegisterData(int headSlot, DrawLayerData data)
    {
        if (!HeadLayerData.ContainsKey(headSlot)) HeadLayerData.Add(headSlot, data);
    }

    public override void Load()
    {
        HeadLayerData = new Dictionary<int, DrawLayerData>();
    }

    public override void Unload()
    {
        HeadLayerData = null;
    }

    public override Position GetDefaultPosition()
    {
        return new AfterParent(PlayerDrawLayers.Head);
    }

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        var basePlayer = drawInfo.drawPlayer.GetModPlayer<BasePlayer>();
        var ironManPlayer = drawInfo.drawPlayer.GetModPlayer<IronManPlayer>();

        return (basePlayer.transformation == MarvelTerrariaUniverse.Transformation.IronMan && ironManPlayer.Mark > 1) || drawInfo.drawPlayer == UITransformationCharacter.preview;
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var drawPlayer = drawInfo.drawPlayer;

        if (!HeadLayerData.TryGetValue(drawPlayer.head, out DrawLayerData data)) return;

        var drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.bodyFrame.Width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition;
        var headVect = drawInfo.headVect;
        var color = drawPlayer.GetImmuneAlphaPure(data.Color(drawInfo), drawInfo.shadow);

        DrawData drawData = new(data.Texture.Value, drawPos.Floor() + headVect, drawPlayer.bodyFrame, color, drawPlayer.headRotation, headVect, 1f, drawInfo.playerEffect, 0);
        drawInfo.DrawDataCache.Add(drawData);
    }
}