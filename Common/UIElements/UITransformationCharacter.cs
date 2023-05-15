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

    public void ChangeDirection(int direction)
    {
        if (direction != -1 || direction != -1) return;

        preview.direction = direction;
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


        /*SpriteSortMode sortMode = Get<SpriteSortMode>(spriteBatch, "sortMode");
        BlendState blendState = Get<BlendState>(spriteBatch, "blendState");
        SamplerState samplerState = Get<SamplerState>(spriteBatch, "samplerState");
        DepthStencilState depthStencilState = Get<DepthStencilState>(spriteBatch, "depthStencilState");
        RasterizerState rasterizerState = Get<RasterizerState>(spriteBatch, "rasterizerState");
        Matrix transformMatrix = Get<Matrix>(spriteBatch, "transformMatrix");
        Effect customEffect = Get<Effect>(spriteBatch, "customEffect");
        T Get<T>(object instance, string name) => (T)instance.GetType().GetField(name, unchecked((System.Reflection.BindingFlags)(-1))).GetValue(instance);

        var gd = spriteBatch.GraphicsDevice;
        var cliprect = gd.ScissorRectangle;
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, blendState, samplerState, depthStencilState, RasterizerState.CullNone, customEffect, transformMatrix);


        DrawData drawData = new(rt.GetTarget(), GetOuterDimensions().Center() - Main.LocalPlayer.Size / 2 - new Vector2(4, 0), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);

        Rectangle rect = Main.LocalPlayer.Hitbox with { X = 0, Y = 0 };
        rect.Width *= 4;
        rect.Height *= 2;

        float xRatio = Main.LocalPlayer.width / (float)drawData.texture.Width;
        float yRatio = Main.LocalPlayer.height / (float)drawData.texture.Height;
        VertexPositionColorTexture TL = new(new Vector3(drawData.position, 0), drawData.color, Vector2.Zero); // top left
        VertexPositionColorTexture TR = new(new Vector3(drawData.position + new Vector2(rect.Width, 0) * drawData.scale, 0), drawData.color, new Vector2(xRatio, 0)); // top right
        VertexPositionColorTexture BL = new(new Vector3(drawData.position + new Vector2(0, rect.Height) * drawData.scale, 0), drawData.color, new Vector2(0, yRatio)); // bottom left

        Vector2 lerpProgress = new Vector2(0.7f, 0.5f);//with
        Vector2 lerpProgress1 = new Vector2(1, lerpProgress.Y);
        Vector2 lerpProgress2 = new Vector2(lerpProgress.X, 1);
        Vector2 offset1 = rect.Size() * lerpProgress1 * drawData.scale; // :/ im tryna get bitches ill help laterMathHelper.
        Vector2 offset2 = rect.Size() * lerpProgress2 * drawData.scale;
        VertexPositionColorTexture BR1 = new(new Vector3(drawData.position + offset1, 0), drawData.color, lerpProgress1 * new Vector2(xRatio, 0)); // the one on the right
        VertexPositionColorTexture BR2 = new(new Vector3(drawData.position + offset2, 0), drawData.color, lerpProgress2 * new Vector2(0, yRatio)); // the one below

        VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[] { BL, TL, TR, BR1, BR2 };
        gd.Textures[0] = drawData.texture;
        gd.ScissorRectangle = cliprect;
        gd.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length - 2);

        spriteBatch.End();

        spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, customEffect, transformMatrix);
        */




        spriteBatch.Draw(rt.GetTarget(), GetOuterDimensions().Center() - Main.LocalPlayer.Size / 2 - new Vector2(4, 0), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
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
        ////Asset<Texture2D> asset = TextureAssets.Extra[171];
        //spriteBatch.Begin(); // main engine crash baby, how romantic
        //spriteBatch.Draw(TextureAssets.MagicPixel.Value, Vector2.Zero, null, Color.White);
        //spriteBatch.End(); // main engine crash baby, how romantic
        PrepareARenderTarget_AndListenToEvents(ref _target, device, 84, 84, RenderTargetUsage.PreserveContents);
        device.SetRenderTarget(_target);
        device.Clear(Color.Transparent);

        preview.head = EquipLoader.GetEquipSlot(Mod, transformation, EquipType.Head);
        preview.body = EquipLoader.GetEquipSlot(Mod, transformation, EquipType.Body);
        preview.legs = EquipLoader.GetEquipSlot(Mod, transformation, EquipType.Legs);

        // Terraria.Graphics.Camera camera = new(); // smile bae :)
        spriteBatch.Begin();
        Vector2 origpos = Main.screenPosition;
        Main.screenPosition = new Vector2(-4, 0);
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
