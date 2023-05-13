using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace MarvelTerrariaUniverse.Common.UIElements;
public class UITransformationCharacter : UICharacter
{
    public static Player preview = (Player)new Player().Clone();

    public string transformation;
    public float scale;

    public UITransformationCharacter(string transformation, float scale = 1f) : base(preview, hasBackPanel: false)
    {
        this.transformation = transformation;
        this.scale = scale;
    }

    Mod Mod => ModContent.GetInstance<MarvelTerrariaUniverse>();

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        var rt = UITransformationCharacterRenderTargetsManager.GetRendertargetForTransform(transformation);
        if (!rt.IsReady)
        {
            Main.NewText("Not ready yet");
            return;
        }
        spriteBatch.Draw(TextureAssets.MagicPixel.Value, GetOuterDimensions().Position(), Color.White);
        spriteBatch.Draw(rt.GetTarget(), GetOuterDimensions().Center() - rt.GetTarget().Size() / new Vector2(8, 8), Color.White);
        //Main.NewText("Ready now");

        //Main.PlayerRenderer.DrawPlayer(Main.Camera, preview, GetOuterDimensions().Position(), 0f, GetOuterDimensions().Center(), 0, scale);
    }
}

class UITransformationCharacterRenderTargetsManager : ModSystem
{
    internal static Dictionary<string, RequestDeez> renderTargetContents = new();

    public static RequestDeez GetRendertargetForTransform(string transformationName)
    {
        if (!renderTargetContents.TryGetValue(transformationName, out var value))
        {
            value = renderTargetContents[transformationName] = new(transformationName);
            Main.ContentThatNeedsRenderTargets.Add(value);
            value.Request();
        }
        return value;
    }

    public override void Load()
    {
        base.Load();
    }

    public override void UpdateUI(GameTime gameTime)
    {
        foreach (var content in renderTargetContents.Values)
            content.Request();
    }

    public override void Unload()
    {
        base.Unload();
        foreach (var content in renderTargetContents)
        {
            Main.ContentThatNeedsRenderTargets.Remove(content.Value);
        }
        renderTargetContents.Clear();
    }
}

class RequestDeez : ARenderTargetContentByRequest
{
    private static Mod Mod => ModContent.GetInstance<MarvelTerrariaUniverse>();
    private static Player preview => UITransformationCharacter.preview;
    public string transformation;

    public RequestDeez(string targetTransformation)
    {
        transformation = targetTransformation;
    }

    protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
    {
        //Asset<Texture2D> asset = TextureAssets.Extra[171];
        float s = 2f;
        PrepareARenderTarget_AndListenToEvents(ref _target, device, (int)(42 * s), (int)(42 * s), RenderTargetUsage.PreserveContents);
        device.SetRenderTarget(_target);
        device.Clear(Color.Transparent);

        spriteBatch.Begin();
        preview.head = EquipLoader.GetEquipSlot(Mod, transformation, EquipType.Head);
        preview.body = EquipLoader.GetEquipSlot(Mod, transformation, EquipType.Body);
        preview.legs = EquipLoader.GetEquipSlot(Mod, transformation, EquipType.Legs);

        // Terraria.Graphics.Camera camera = new(); // smile bae :)
        Vector2 origpos = Main.screenPosition;
        Main.screenPosition = Vector2.Zero;
        Main.PlayerRenderer.DrawPlayer(Main.Camera, preview, Vector2.Zero, 0f, preview.Size / 2f, 0, 1f);
        Main.screenPosition = origpos;

        spriteBatch.End();

        /*DrawData value = new(asset.Value, Vector2.Zero, Color.White);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        GameShaders.Misc["HallowBoss"].Apply(value);
        value.Draw(spriteBatch);
        spriteBatch.End();*/
        device.SetRenderTarget(null);
        _wasPrepared = true;
        Request();
    }
}
