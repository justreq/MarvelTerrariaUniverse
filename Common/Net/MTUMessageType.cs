using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace MarvelTerrariaUniverse.Common.Net;
enum MTUMessageType
{
    Unknown,
    EquipSlotChange,
}

interface INetPacket : ILoadable
{
    MTUMessageType MessageType { get; }
    void Write(BinaryWriter writer);
    void ILoadable.Load(Mod mod) { }
    void ILoadable.Unload() { }
}
// note: Message type is excluded from the reader
/// <summary>Defines a type for net packets.</summary>
/// <remarks>Classes and record classes should define at least one parameterless constructor, however structs dont require it.</remarks>
/// <typeparam name="TSelf"></typeparam>
interface INetPacket<TSelf> : INetPacket
{
    /// <summary>
    /// Creates a <see cref="TSelf"/> instance from binary.<br />
    /// </summary>
    /// <remarks>The message type is not included in the reader.</remarks>
    TSelf Read(BinaryReader reader);
}

internal record struct EquipSlotChangePacket(int Player, int PlayerHead, int PlayerBody, int PlayerLegs) : INetPacket<EquipSlotChangePacket>
{
    public MTUMessageType MessageType => MTUMessageType.EquipSlotChange;

    public readonly EquipSlotChangePacket Read(BinaryReader reader) => new EquipSlotChangePacket(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

    public readonly void Write(BinaryWriter writer)
    {
        writer.Write(Player);
        writer.Write(PlayerHead);
        writer.Write(PlayerBody);
        writer.Write(PlayerLegs);
    }
}
// 

internal static class MessageHelpers
{
    public static void Send<T>(this T message, int toClient = -1, int ignoreClient = -1) where T : INetPacket<T>
    {
        if (Main.netMode == NetmodeID.SinglePlayer) return;
        ModPacket packet = MTUNetMessages.CreatePacket(message.MessageType);
        message.Write(packet);
        packet.Send();
    }
}

class MTUNetMessages : ILoadable
{
    private static Dictionary<MTUMessageType, Action<BinaryReader, int>> messageHandlers = new();
    public static ModPacket CreatePacket(MTUMessageType type, int? capacity = null)
    {
        ModPacket packet = MarvelTerrariaUniverse.Instance.GetPacket(capacity ?? 256);
        packet.Write((int)type);
        return packet;
    }
    internal static void HandlePacket(BinaryReader reader, int fromWho)
    {
        try
        {
            MTUMessageType type = (MTUMessageType)reader.ReadInt32();
            if (type == MTUMessageType.Unknown)
                Console.WriteLine("Recieved unknown message");
            else
                messageHandlers[type](reader, fromWho);
        }
        catch
        {

        }
    }
    public void Load(Mod mod)
    {
        foreach (var packet in AssemblyManager.GetLoadableTypes(mod.Code).Where(t => t is { IsInterface: false } && typeof(INetPacket).IsAssignableFrom(t)).Select(Activator.CreateInstance).Cast<INetPacket>())
        {

            var packetType = packet.GetType();
            var type = typeof(MTUNetMessages<>).MakeGenericType(packetType);
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
            if (packetType.IsClass)
                type.GetMethod("Load", flags).Invoke(null, new object[] { packet });

            messageHandlers[packet.MessageType] = type.GetMethod("Handle", flags).CreateDelegate<Action<BinaryReader, int>>();
        }
    }
    public void Unload()
    {
        messageHandlers?.Clear();
        messageHandlers = null;
    }
}

delegate void MessageRecieve<T>(in T data, int fromWho);
static class MTUNetMessages<T> where T : INetPacket<T>
{
    static T singleton = default;
    public static event MessageRecieve<T> OnRecieve;
    internal static void Handle(BinaryReader reader, int fromWho)
    {
        OnRecieve?.Invoke(singleton.Read(reader), fromWho);
    }
    internal static void Load(object instance)
    {
        singleton = (T)instance;
    }
}