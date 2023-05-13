using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Core.Effects;
public class ParticleEntitySystem : ModSystem
{
    private static Dictionary<Type, LinkedList<ParticleEntity>> entitiesByType = new();
    public static ParticleEntity[] particleEntities = new ParticleEntity[6001];
    private static int particleIndex;

    public override void Load()
    {
        On_Main.DrawProjectiles += (orig, self) =>
        {
            DrawBackEntities();

            orig(self);

            DrawFrontEntities();
        };


        for (int i = 0; i < particleEntities.Length; i++)
        {
            particleEntities[i] = new ParticleEntity();
        }
    }

    public override void Unload()
    {
        entitiesByType?.Clear();
    }

    public override void PreUpdateEntities() => UpdateEntities();

    public static IEnumerable<ParticleEntity> EnumerateEntities()
    {
        foreach (var entities in entitiesByType.Values)
        {
            var node = entities.First;

            while (node != null)
            {
                var entity = node.Value;

                if (entity.Destroyed)
                {
                    var next = node.Next;

                    entities.Remove(node);

                    node = next;
                }
                else
                {
                    yield return entity;

                    node = node.Next;
                }
            }
        }
    }

    internal static T InstantiateEntity<T>(Action<T>? preinitializer = null) where T : ParticleEntity
    {
        T instance = Activator.CreateInstance<T>();

        while (!particleEntities[particleIndex].Destroyed)
            particleIndex++;
        if (particleIndex >= particleEntities.Length - 2)
            particleIndex = 0;
        preinitializer?.Invoke(instance);
        particleEntities[particleIndex] = instance;
        particleEntities[particleIndex].Destroyed = false;
        particleEntities[particleIndex].Init();



        return instance;
    }

    private static void UpdateEntities()
    {
        if (!Main.dedServ && Main.gamePaused)
        {
            return;
        }

        for (int i = 0; i < particleEntities.Length - 1; i++)
        {
            if (particleEntities[i].Destroyed)
            {
                particleEntities[i] = new ParticleEntity();
                particleEntities[i].Destroyed = true;
            }
            else
            {
                particleEntities[i].Update();
            }
        }
    }

    private static void DrawFrontEntities()
    {
        var sb = Main.spriteBatch;

        sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

        for (int i = 0; i < particleEntities.Length - 1; i++)
        {

            if (!particleEntities[i].Destroyed && !particleEntities[i].back)
            {
                if (!particleEntities[i].additive)
                    particleEntities[i].Draw(sb);
            }
        }

        sb.End();

        sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

        for (int i = 0; i < particleEntities.Length - 1; i++)
        {
            if (!particleEntities[i].Destroyed && !particleEntities[i].back)
            {

                if (particleEntities[i].additive)
                {

                    particleEntities[i].Draw(sb);
                }
            }
        }
        sb.End();
    }

    private static void DrawBackEntities()
    {
        var sb = Main.spriteBatch;

        sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

        for (int i = 0; i < particleEntities.Length - 1; i++)
        {

            if (!particleEntities[i].Destroyed && particleEntities[i].back)
            {
                if (!particleEntities[i].additive)
                    particleEntities[i].Draw(sb);
            }
        }

        sb.End();

        sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

        for (int i = 0; i < particleEntities.Length - 1; i++)
        {
            if (!particleEntities[i].Destroyed && particleEntities[i].back)
            {

                if (particleEntities[i].additive)
                {

                    particleEntities[i].Draw(sb);
                }
            }
        }
        sb.End();
    }
}