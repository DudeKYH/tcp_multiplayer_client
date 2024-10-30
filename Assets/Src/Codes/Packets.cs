using UnityEngine;
using ProtoBuf;
using System.IO;
using System.Buffers;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

public class Packets : MonoBehaviour
{
    public enum PacketType { Ping, Normal, Location = 3 }
    public enum HandlerIds {
        Init = 0,
        LocationUpdate = 2 
    }

    public static void Serialize<T>(IBufferWriter<byte> writer, T data)
    {
        Serializer.Serialize(writer, data);
    }

    public static T Deserialize<T>(byte[] data) {
        try {
            using (var stream = new MemoryStream(data)) {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        } catch (Exception ex) {
            Debug.LogError($"Deserialize: Failed to deserialize data. Exception: {ex}");
            throw;
        }
    }
}

[ProtoContract]
public class InitialPayload
{
    [ProtoMember(1, IsRequired = true)]
    public string deviceId { get; set; }

    [ProtoMember(2, IsRequired = true)]
    public uint playerId { get; set; }
    
    [ProtoMember(3, IsRequired = true)]
    public float latency { get; set; }
}

[ProtoContract]
public class CommonPacket
{
    [ProtoMember(1)]
    public uint handlerId { get; set; }

    [ProtoMember(2)]
    public string userId { get; set; }

    [ProtoMember(3)]
    public string version { get; set; }

    [ProtoMember(4)]
    public byte[] payload { get; set; }
}

[ProtoContract]
public class LocationUpdatePayload {
    [ProtoMember(1, IsRequired = true)]
    public float x { get; set; }
    [ProtoMember(2, IsRequired = true)]
    public float y { get; set; }
    [ProtoMember(3, IsRequired = true)]
    public float velX { get; set; }

    [ProtoMember(4, IsRequired = true)]
    public float velY { get; set; }
}

[ProtoContract]
public class LocationUpdate
{
    [ProtoMember(1)]
    public List<UserLocation> users { get; set; }

    [ProtoContract]
    public class UserLocation
    {
        [ProtoMember(1)]
        public string id { get; set; }

        [ProtoMember(2)]
        public uint playerId { get; set; }

        [ProtoMember(3)]
        public float x { get; set; }

        [ProtoMember(4)]
        public float y { get; set; }

        [ProtoMember(5)]
        public float velX { get; set; }

        [ProtoMember(6)]
        public float velY { get; set; }
    }
}

[ProtoContract]
public class Response {
    [ProtoMember(1)]
    public uint handlerId { get; set; }

    [ProtoMember(2)]
    public uint responseCode { get; set; }

    [ProtoMember(3)]
    public long timestamp { get; set; }

    [ProtoMember(4)]
    public byte[] data { get; set; }
}

[ProtoContract]
public class Ping
{
    [ProtoMember(1)]
    public ulong timestamp { get; set; }
}

[DataContract]
public class InitialResponseData
{
    [DataMember]
    public string userId { get; set; }

    [DataMember]
    public float x { get; set; }

    [DataMember]
    public float y { get; set; }
}
