﻿using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefPrefixedString : ProtodefType
{
    [JsonConstructor]
    public ProtodefPrefixedString(ProtodefType countType)
    {
        CountType = countType;
    }

    [JsonPropertyName("countType")] public ProtodefType CountType { get; }

    public override string? GetNetType()
    {
        return "string";
    }

    private bool Equals(ProtodefPrefixedString other)
    {
        return CountType.Equals(other.CountType);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefPrefixedString other && Equals(other);
    }

    public override int GetHashCode()
    {
        return CountType.GetHashCode();
    }
}