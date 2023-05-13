using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.AssetManager;

public partial class Assets
{
    public const string Path = $"{nameof(MarvelTerrariaUniverse)}/Assets";

    public static SoundStyle ToSoundStyle(string path) => new(path);

    public static Asset<Texture2D> ToTexture2D(string path) => ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad);

    public partial class Sounds
    {
        public const string Path = $"{Assets.Path}/Sounds";
    }

    public partial class Textures
    {
        public const string Path = $"{Assets.Path}/Textures";
    }
}